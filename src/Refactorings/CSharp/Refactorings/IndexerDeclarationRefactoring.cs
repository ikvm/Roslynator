﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CSharp.Analysis;
using Roslynator.CSharp.Refactorings.MakeMemberAbstract;
using Roslynator.CSharp.Refactorings.MakeMemberVirtual;

namespace Roslynator.CSharp.Refactorings
{
    internal static class IndexerDeclarationRefactoring
    {
        public static async Task ComputeRefactoringsAsync(RefactoringContext context, IndexerDeclarationSyntax indexerDeclaration)
        {
            if (context.IsRefactoringEnabled(RefactoringIdentifiers.UseExpressionBodiedMember)
                && context.SupportsCSharp6)
            {
                AccessorListSyntax accessorList = indexerDeclaration.AccessorList;

                if (accessorList != null
                    && context.Span.IsEmptyAndContainedInSpanOrBetweenSpans(accessorList))
                {
                    AccessorDeclarationSyntax accessor = indexerDeclaration
                        .AccessorList?
                        .Accessors
                        .SingleOrDefault(shouldThrow: false);

                    if (accessor?.AttributeLists.Any() == false
                            && accessor.IsKind(SyntaxKind.GetAccessorDeclaration)
                            && accessor.Body != null
                            && (UseExpressionBodiedMemberAnalysis.GetReturnExpression(accessor.Body) != null))
                    {
                        context.RegisterRefactoring(
                            UseExpressionBodiedMemberRefactoring.Title,
                            ct => UseExpressionBodiedMemberRefactoring.RefactorAsync(context.Document, indexerDeclaration, ct),
                            RefactoringIdentifiers.UseExpressionBodiedMember);
                    }
                }
            }

            if (context.IsRefactoringEnabled(RefactoringIdentifiers.MakeMemberAbstract)
                && indexerDeclaration.HeaderSpan().Contains(context.Span))
            {
                MakeIndexerAbstractRefactoring.ComputeRefactoring(context, indexerDeclaration);
            }

            if (context.IsRefactoringEnabled(RefactoringIdentifiers.MakeMemberVirtual)
                && indexerDeclaration.HeaderSpan().Contains(context.Span))
            {
                MakeIndexerVirtualRefactoring.ComputeRefactoring(context, indexerDeclaration);
            }

            if (context.IsRefactoringEnabled(RefactoringIdentifiers.CopyDocumentationCommentFromBaseMember)
                && indexerDeclaration.HeaderSpan().Contains(context.Span)
                && !indexerDeclaration.HasDocumentationComment())
            {
                SemanticModel semanticModel = await context.GetSemanticModelAsync().ConfigureAwait(false);
                CopyDocumentationCommentFromBaseMemberRefactoring.ComputeRefactoring(context, indexerDeclaration, semanticModel);
            }

            if (context.IsRefactoringEnabled(RefactoringIdentifiers.AddMemberToInterface)
                && context.Span.IsEmptyAndContainedInSpanOrBetweenSpans(indexerDeclaration.ThisKeyword))
            {
                SemanticModel semanticModel = await context.GetSemanticModelAsync().ConfigureAwait(false);

                AddMemberToInterfaceRefactoring.ComputeRefactoring(context, indexerDeclaration, semanticModel);
            }
        }
    }
}