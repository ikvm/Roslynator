﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Roslynator.CSharp.Refactorings
{
    internal static class AvoidSingleLineBlockRefactoring
    {
        public static Task<Document> RefactorAsync(
            Document document,
            BlockSyntax block,
            CancellationToken cancellationToken)
        {
            SyntaxToken closeBrace = block.CloseBraceToken;

            BlockSyntax newBlock = block
                .WithCloseBraceToken(closeBrace.WithLeadingTrivia(closeBrace.LeadingTrivia.Add(CSharpFactory.NewLine())))
                .WithFormatterAnnotation();

            return document.ReplaceNodeAsync(block, newBlock, cancellationToken);
        }
    }
}
