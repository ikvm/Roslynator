﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Immutable;
using System.Composition;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CodeFixes;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Roslynator.CSharp.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(RemoveRedundantAsyncAwaitCodeFixProvider))]
    [Shared]
    public class RemoveRedundantAsyncAwaitCodeFixProvider : BaseCodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(DiagnosticIdentifiers.RemoveRedundantAsyncAwait); }
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            SyntaxNode root = await context.GetSyntaxRootAsync().ConfigureAwait(false);

            if (!TryFindToken(root, context.Span.Start, out SyntaxToken token))
                return;

            Debug.Assert(token.IsKind(SyntaxKind.AsyncKeyword), token.Kind().ToString());

            Diagnostic diagnostic = context.Diagnostics[0];

            CodeAction codeAction = CodeAction.Create(
                "Remove redundant async/await",
                cancellationToken => RefactorAsync(context.Document, token, cancellationToken),
                GetEquivalenceKey(diagnostic));

            context.RegisterCodeFix(codeAction, diagnostic);
        }

        private static async Task<Document> RefactorAsync(
            Document document,
            SyntaxToken token,
            CancellationToken cancellationToken)
        {
            SemanticModel semanticModel = await document.GetSemanticModelAsync(cancellationToken).ConfigureAwait(false);

            SyntaxNode node = token.Parent;

            var remover = new AwaitRemover(semanticModel, cancellationToken);

            SyntaxNode newNode = GetNewNode();

            return await document.ReplaceNodeAsync(node, newNode, cancellationToken).ConfigureAwait(false);

            SyntaxNode GetNewNode()
            {
                switch (node.Kind())
                {
                    case SyntaxKind.MethodDeclaration:
                        {
                            var methodDeclaration = (MethodDeclarationSyntax)node;

                            return remover
                                .VisitMethodDeclaration(methodDeclaration)
                                .RemoveModifier(SyntaxKind.AsyncKeyword);
                        }
                    case SyntaxKind.LocalFunctionStatement:
                        {
                            var localFunction = (LocalFunctionStatementSyntax)node;

                            BlockSyntax body = localFunction.Body;

                            if (body != null)
                            {
                                localFunction = localFunction.WithBody((BlockSyntax)remover.VisitBlock(body));
                            }
                            else
                            {
                                ArrowExpressionClauseSyntax expressionBody = localFunction.ExpressionBody;

                                if (expressionBody != null)
                                    localFunction = localFunction.WithExpressionBody((ArrowExpressionClauseSyntax)remover.VisitArrowExpressionClause(expressionBody));
                            }

                            return localFunction.RemoveModifier(SyntaxKind.AsyncKeyword);
                        }
                    case SyntaxKind.SimpleLambdaExpression:
                        {
                            var lambda = (SimpleLambdaExpressionSyntax)node;

                            return lambda
                                .WithBody((CSharpSyntaxNode)remover.Visit(lambda.Body))
                                .WithAsyncKeyword(GetMissingAsyncKeyword(lambda.AsyncKeyword));
                        }
                    case SyntaxKind.ParenthesizedLambdaExpression:
                        {
                            var lambda = (ParenthesizedLambdaExpressionSyntax)node;

                            return lambda
                                .WithBody((CSharpSyntaxNode)remover.Visit(lambda.Body))
                                .WithAsyncKeyword(GetMissingAsyncKeyword(lambda.AsyncKeyword));
                        }
                    case SyntaxKind.AnonymousMethodExpression:
                        {
                            var anonymousMethod = (AnonymousMethodExpressionSyntax)node;

                            return anonymousMethod
                                .WithBody((CSharpSyntaxNode)remover.Visit(anonymousMethod.Body))
                                .WithAsyncKeyword(GetMissingAsyncKeyword(anonymousMethod.AsyncKeyword));
                        }
                }

                throw new InvalidOperationException();
            }
        }

        private static SyntaxToken GetMissingAsyncKeyword(SyntaxToken asyncKeyword)
        {
            if (asyncKeyword.TrailingTrivia.All(f => f.IsWhitespaceTrivia()))
            {
                return MissingToken(SyntaxKind.AsyncKeyword).WithLeadingTrivia(asyncKeyword.LeadingTrivia);
            }
            else
            {
                return MissingToken(SyntaxKind.AsyncKeyword).WithTriviaFrom(asyncKeyword);
            }
        }

        private class AwaitRemover : CSharpSyntaxRewriter
        {
            public AwaitRemover(SemanticModel semanticModel, CancellationToken cancellationToken)
            {
                SemanticModel = semanticModel;
                CancellationToken = cancellationToken;
            }

            public SemanticModel SemanticModel { get; }

            public CancellationToken CancellationToken { get; }

            private static ExpressionSyntax ExtractExpressionFromAwait(AwaitExpressionSyntax awaitExpression, SemanticModel semanticModel, CancellationToken cancellationToken)
            {
                ExpressionSyntax expression = awaitExpression.Expression;

                if (semanticModel.GetTypeSymbol(expression, cancellationToken) is INamedTypeSymbol typeSymbol)
                {
                    if (typeSymbol.HasMetadataName(MetadataNames.System_Runtime_CompilerServices_ConfiguredTaskAwaitable)
                        || typeSymbol.OriginalDefinition.HasMetadataName(MetadataNames.System_Runtime_CompilerServices_ConfiguredTaskAwaitable_T))
                    {
                        if (expression is InvocationExpressionSyntax invocation)
                        {
                            var memberAccess = invocation.Expression as MemberAccessExpressionSyntax;

                            if (string.Equals(memberAccess?.Name?.Identifier.ValueText, "ConfigureAwait", StringComparison.Ordinal))
                                expression = memberAccess.Expression;
                        }
                    }
                }

                return expression.WithTriviaFrom(awaitExpression);
            }

            public override SyntaxNode VisitAwaitExpression(AwaitExpressionSyntax node)
            {
                node = (AwaitExpressionSyntax)base.VisitAwaitExpression(node);

                return ExtractExpressionFromAwait(node, SemanticModel, CancellationToken);
            }

            public override SyntaxNode VisitSimpleLambdaExpression(SimpleLambdaExpressionSyntax node)
            {
                return node;
            }

            public override SyntaxNode VisitParenthesizedLambdaExpression(ParenthesizedLambdaExpressionSyntax node)
            {
                return node;
            }

            public override SyntaxNode VisitAnonymousMethodExpression(AnonymousMethodExpressionSyntax node)
            {
                return node;
            }

            public override SyntaxNode VisitLocalFunctionStatement(LocalFunctionStatementSyntax node)
            {
                return node;
            }
        }
    }
}
