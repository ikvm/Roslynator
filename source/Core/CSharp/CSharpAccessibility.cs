﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Roslynator.CSharp.Comparers;
using Roslynator.CSharp.Syntax;

namespace Roslynator.CSharp
{
    public static class CSharpAccessibility
    {
        public static Accessibility GetDefaultAccessibility(MemberDeclarationSyntax member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            switch (member.Kind())
            {
                case SyntaxKind.ConstructorDeclaration:
                    return GetDefaultAccessibility((ConstructorDeclarationSyntax)member);
                case SyntaxKind.DestructorDeclaration:
                    return GetDefaultAccessibility((DestructorDeclarationSyntax)member);
                case SyntaxKind.MethodDeclaration:
                    return GetDefaultAccessibility((MethodDeclarationSyntax)member);
                case SyntaxKind.PropertyDeclaration:
                    return GetDefaultAccessibility((PropertyDeclarationSyntax)member);
                case SyntaxKind.IndexerDeclaration:
                    return GetDefaultAccessibility((IndexerDeclarationSyntax)member);
                case SyntaxKind.EventDeclaration:
                    return GetDefaultAccessibility((EventDeclarationSyntax)member);
                case SyntaxKind.EventFieldDeclaration:
                    return GetDefaultAccessibility((EventFieldDeclarationSyntax)member);
                case SyntaxKind.FieldDeclaration:
                    return Accessibility.Private;
                case SyntaxKind.OperatorDeclaration:
                case SyntaxKind.ConversionOperatorDeclaration:
                    return Accessibility.Public;
                case SyntaxKind.ClassDeclaration:
                case SyntaxKind.StructDeclaration:
                case SyntaxKind.InterfaceDeclaration:
                case SyntaxKind.EnumDeclaration:
                    return GetDefaultAccessibility((BaseTypeDeclarationSyntax)member);
                case SyntaxKind.DelegateDeclaration:
                    return GetDefaultAccessibility((DelegateDeclarationSyntax)member);
                case SyntaxKind.EnumMemberDeclaration:
                case SyntaxKind.NamespaceDeclaration:
                    return Accessibility.Public;
            }

            Debug.Fail(member.Kind().ToString());

            return Accessibility.NotApplicable;
        }

        public static Accessibility GetDefaultAccessibility(ClassDeclarationSyntax classDeclaration)
        {
            return GetDefaultAccessibility((TypeDeclarationSyntax)classDeclaration);
        }

        public static Accessibility GetDefaultAccessibility(ConstructorDeclarationSyntax constructorDeclaration)
        {
            if (constructorDeclaration == null)
                throw new ArgumentNullException(nameof(constructorDeclaration));

            if (constructorDeclaration.Modifiers.Contains(SyntaxKind.StaticKeyword))
            {
                return Accessibility.Public;
            }
            else
            {
                return Accessibility.Private;
            }
        }

        public static Accessibility GetDefaultAccessibility(ConversionOperatorDeclarationSyntax conversionOperatorDeclaration)
        {
            if (conversionOperatorDeclaration == null)
                throw new ArgumentNullException(nameof(conversionOperatorDeclaration));

            return Accessibility.Public;
        }

        public static Accessibility GetDefaultAccessibility(DelegateDeclarationSyntax delegateDeclaration)
        {
            if (delegateDeclaration == null)
                throw new ArgumentNullException(nameof(delegateDeclaration));

            if (delegateDeclaration.IsParentKind(SyntaxKind.ClassDeclaration, SyntaxKind.StructDeclaration))
            {
                return Accessibility.Private;
            }
            else
            {
                return Accessibility.Internal;
            }
        }

        public static Accessibility GetDefaultAccessibility(DestructorDeclarationSyntax destructorDeclaration)
        {
            if (destructorDeclaration == null)
                throw new ArgumentNullException(nameof(destructorDeclaration));

            return Accessibility.Public;
        }

        public static Accessibility GetDefaultAccessibility(EnumDeclarationSyntax enumDeclaration)
        {
            return GetDefaultAccessibility((BaseTypeDeclarationSyntax)enumDeclaration);
        }

        public static Accessibility GetDefaultAccessibility(EnumMemberDeclarationSyntax enumMemberDeclaration)
        {
            if (enumMemberDeclaration == null)
                throw new ArgumentNullException(nameof(enumMemberDeclaration));

            return Accessibility.Public;
        }

        public static Accessibility GetDefaultAccessibility(EventDeclarationSyntax eventDeclaration)
        {
            if (eventDeclaration == null)
                throw new ArgumentNullException(nameof(eventDeclaration));

            return Accessibility.Private;
        }

        public static Accessibility GetDefaultAccessibility(EventFieldDeclarationSyntax eventFieldDeclaration)
        {
            if (eventFieldDeclaration == null)
                throw new ArgumentNullException(nameof(eventFieldDeclaration));

            if (eventFieldDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
            {
                return Accessibility.Public;
            }
            else
            {
                return Accessibility.Private;
            }
        }

        public static Accessibility GetDefaultAccessibility(FieldDeclarationSyntax fieldDeclaration)
        {
            if (fieldDeclaration == null)
                throw new ArgumentNullException(nameof(fieldDeclaration));

            return Accessibility.Private;
        }

        public static Accessibility GetDefaultAccessibility(IndexerDeclarationSyntax indexerDeclaration)
        {
            if (indexerDeclaration == null)
                throw new ArgumentNullException(nameof(indexerDeclaration));

            if (indexerDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
            {
                return Accessibility.Public;
            }
            else
            {
                return Accessibility.Private;
            }
        }

        public static Accessibility GetDefaultAccessibility(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            return GetDefaultAccessibility((BaseTypeDeclarationSyntax)interfaceDeclaration);
        }

        public static Accessibility GetDefaultAccessibility(MethodDeclarationSyntax methodDeclaration)
        {
            if (methodDeclaration == null)
                throw new ArgumentNullException(nameof(methodDeclaration));

            if (methodDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
            {
                return Accessibility.Public;
            }
            else
            {
                return Accessibility.Private;
            }
        }

        public static Accessibility GetDefaultAccessibility(NamespaceDeclarationSyntax namespaceDeclaration)
        {
            if (namespaceDeclaration == null)
                throw new ArgumentNullException(nameof(namespaceDeclaration));

            return Accessibility.Public;
        }

        public static Accessibility GetDefaultAccessibility(OperatorDeclarationSyntax operatorDeclaration)
        {
            if (operatorDeclaration == null)
                throw new ArgumentNullException(nameof(operatorDeclaration));

            return Accessibility.Public;
        }

        public static Accessibility GetDefaultAccessibility(PropertyDeclarationSyntax propertyDeclaration)
        {
            if (propertyDeclaration == null)
                throw new ArgumentNullException(nameof(propertyDeclaration));

            if (propertyDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
            {
                return Accessibility.Public;
            }
            else
            {
                return Accessibility.Private;
            }
        }

        public static Accessibility GetDefaultAccessibility(StructDeclarationSyntax structDeclaration)
        {
            return GetDefaultAccessibility((BaseTypeDeclarationSyntax)structDeclaration);
        }

        public static Accessibility GetDefaultAccessibility(BaseTypeDeclarationSyntax baseTypeDeclaration)
        {
            if (baseTypeDeclaration == null)
                throw new ArgumentNullException(nameof(baseTypeDeclaration));

            if (baseTypeDeclaration.IsParentKind(SyntaxKind.ClassDeclaration, SyntaxKind.StructDeclaration))
            {
                return Accessibility.Private;
            }
            else
            {
                return Accessibility.Internal;
            }
        }

        public static Accessibility GetDefaultAccessibility(AccessorDeclarationSyntax accessorDeclaration)
        {
            if (accessorDeclaration == null)
                throw new ArgumentNullException(nameof(accessorDeclaration));

            SyntaxNode declaration = accessorDeclaration.Parent?.Parent;

            switch (declaration?.Kind())
            {
                case SyntaxKind.PropertyDeclaration:
                    return GetDefaultAccessibility((PropertyDeclarationSyntax)declaration);
                case SyntaxKind.IndexerDeclaration:
                    return GetDefaultAccessibility((IndexerDeclarationSyntax)declaration);
                case SyntaxKind.EventDeclaration:
                    return GetDefaultAccessibility((EventDeclarationSyntax)declaration);
            }

            Debug.Assert(declaration == null, declaration.Kind().ToString());

            return Accessibility.NotApplicable;
        }

        public static Accessibility GetDefaultExplicitAccessibility(MemberDeclarationSyntax member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            switch (member.Kind())
            {
                case SyntaxKind.ConstructorDeclaration:
                    return GetDefaultExplicitAccessibility((ConstructorDeclarationSyntax)member);
                case SyntaxKind.DestructorDeclaration:
                    return GetDefaultExplicitAccessibility((DestructorDeclarationSyntax)member);
                case SyntaxKind.MethodDeclaration:
                    return GetDefaultExplicitAccessibility((MethodDeclarationSyntax)member);
                case SyntaxKind.PropertyDeclaration:
                    return GetDefaultExplicitAccessibility((PropertyDeclarationSyntax)member);
                case SyntaxKind.IndexerDeclaration:
                    return GetDefaultExplicitAccessibility((IndexerDeclarationSyntax)member);
                case SyntaxKind.EventDeclaration:
                    return GetDefaultExplicitAccessibility((EventDeclarationSyntax)member);
                case SyntaxKind.EventFieldDeclaration:
                    return GetDefaultExplicitAccessibility((EventFieldDeclarationSyntax)member);
                case SyntaxKind.FieldDeclaration:
                    return Accessibility.Private;
                case SyntaxKind.OperatorDeclaration:
                case SyntaxKind.ConversionOperatorDeclaration:
                    return Accessibility.Public;
                case SyntaxKind.ClassDeclaration:
                case SyntaxKind.StructDeclaration:
                case SyntaxKind.InterfaceDeclaration:
                case SyntaxKind.EnumDeclaration:
                    return GetDefaultExplicitAccessibility((BaseTypeDeclarationSyntax)member);
                case SyntaxKind.DelegateDeclaration:
                    return GetDefaultExplicitAccessibility((DelegateDeclarationSyntax)member);
                case SyntaxKind.EnumMemberDeclaration:
                case SyntaxKind.NamespaceDeclaration:
                    return Accessibility.NotApplicable;
            }

            Debug.Fail(member.Kind().ToString());

            return Accessibility.NotApplicable;
        }

        public static Accessibility GetDefaultExplicitAccessibility(ClassDeclarationSyntax classDeclaration)
        {
            return GetDefaultExplicitAccessibility((TypeDeclarationSyntax)classDeclaration);
        }

        public static Accessibility GetDefaultExplicitAccessibility(ConstructorDeclarationSyntax constructorDeclaration)
        {
            if (constructorDeclaration == null)
                throw new ArgumentNullException(nameof(constructorDeclaration));

            if (constructorDeclaration.Modifiers.Contains(SyntaxKind.StaticKeyword))
            {
                return Accessibility.NotApplicable;
            }
            else
            {
                return Accessibility.Private;
            }
        }

        public static Accessibility GetDefaultExplicitAccessibility(ConversionOperatorDeclarationSyntax conversionOperatorDeclaration)
        {
            if (conversionOperatorDeclaration == null)
                throw new ArgumentNullException(nameof(conversionOperatorDeclaration));

            return Accessibility.Public;
        }

        public static Accessibility GetDefaultExplicitAccessibility(DelegateDeclarationSyntax delegateDeclaration)
        {
            if (delegateDeclaration == null)
                throw new ArgumentNullException(nameof(delegateDeclaration));

            if (delegateDeclaration.IsParentKind(SyntaxKind.ClassDeclaration, SyntaxKind.StructDeclaration))
            {
                return Accessibility.Private;
            }
            else
            {
                return Accessibility.Internal;
            }
        }

        public static Accessibility GetDefaultExplicitAccessibility(DestructorDeclarationSyntax destructorDeclaration)
        {
            if (destructorDeclaration == null)
                throw new ArgumentNullException(nameof(destructorDeclaration));

            return Accessibility.NotApplicable;
        }

        public static Accessibility GetDefaultExplicitAccessibility(EnumDeclarationSyntax enumDeclaration)
        {
            return GetDefaultExplicitAccessibility((BaseTypeDeclarationSyntax)enumDeclaration);
        }

        public static Accessibility GetDefaultExplicitAccessibility(EnumMemberDeclarationSyntax enumMemberDeclaration)
        {
            if (enumMemberDeclaration == null)
                throw new ArgumentNullException(nameof(enumMemberDeclaration));

            return Accessibility.NotApplicable;
        }

        public static Accessibility GetDefaultExplicitAccessibility(EventDeclarationSyntax eventDeclaration)
        {
            if (eventDeclaration == null)
                throw new ArgumentNullException(nameof(eventDeclaration));

            if (eventDeclaration.ExplicitInterfaceSpecifier != null)
            {
                return Accessibility.NotApplicable;
            }
            else
            {
                return Accessibility.Private;
            }
        }

        public static Accessibility GetDefaultExplicitAccessibility(EventFieldDeclarationSyntax eventFieldDeclaration)
        {
            if (eventFieldDeclaration == null)
                throw new ArgumentNullException(nameof(eventFieldDeclaration));

            if (eventFieldDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
            {
                return Accessibility.NotApplicable;
            }
            else
            {
                return Accessibility.Private;
            }
        }

        public static Accessibility GetDefaultExplicitAccessibility(FieldDeclarationSyntax fieldDeclaration)
        {
            if (fieldDeclaration == null)
                throw new ArgumentNullException(nameof(fieldDeclaration));

            return Accessibility.Private;
        }

        public static Accessibility GetDefaultExplicitAccessibility(IndexerDeclarationSyntax indexerDeclaration)
        {
            if (indexerDeclaration == null)
                throw new ArgumentNullException(nameof(indexerDeclaration));

            if (indexerDeclaration.ExplicitInterfaceSpecifier != null
                || indexerDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
            {
                return Accessibility.NotApplicable;
            }
            else
            {
                return Accessibility.Private;
            }
        }

        public static Accessibility GetDefaultExplicitAccessibility(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            return GetDefaultExplicitAccessibility((BaseTypeDeclarationSyntax)interfaceDeclaration);
        }

        public static Accessibility GetDefaultExplicitAccessibility(MethodDeclarationSyntax methodDeclaration)
        {
            if (methodDeclaration == null)
                throw new ArgumentNullException(nameof(methodDeclaration));

            if (methodDeclaration.Modifiers.Contains(SyntaxKind.PartialKeyword)
                || methodDeclaration.ExplicitInterfaceSpecifier != null
                || methodDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
            {
                return Accessibility.NotApplicable;
            }
            else
            {
                return Accessibility.Private;
            }
        }

        public static Accessibility GetDefaultExplicitAccessibility(NamespaceDeclarationSyntax namespaceDeclaration)
        {
            if (namespaceDeclaration == null)
                throw new ArgumentNullException(nameof(namespaceDeclaration));

            return Accessibility.NotApplicable;
        }

        public static Accessibility GetDefaultExplicitAccessibility(OperatorDeclarationSyntax operatorDeclaration)
        {
            if (operatorDeclaration == null)
                throw new ArgumentNullException(nameof(operatorDeclaration));

            return Accessibility.Public;
        }

        public static Accessibility GetDefaultExplicitAccessibility(PropertyDeclarationSyntax propertyDeclaration)
        {
            if (propertyDeclaration == null)
                throw new ArgumentNullException(nameof(propertyDeclaration));

            if (propertyDeclaration.ExplicitInterfaceSpecifier != null
                || propertyDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
            {
                return Accessibility.NotApplicable;
            }
            else
            {
                return Accessibility.Private;
            }
        }

        public static Accessibility GetDefaultExplicitAccessibility(StructDeclarationSyntax structDeclaration)
        {
            return GetDefaultExplicitAccessibility((BaseTypeDeclarationSyntax)structDeclaration);
        }

        public static Accessibility GetDefaultExplicitAccessibility(BaseTypeDeclarationSyntax baseTypeDeclaration)
        {
            if (baseTypeDeclaration == null)
                throw new ArgumentNullException(nameof(baseTypeDeclaration));

            if (baseTypeDeclaration.IsParentKind(SyntaxKind.ClassDeclaration, SyntaxKind.StructDeclaration))
            {
                return Accessibility.Private;
            }
            else
            {
                return Accessibility.Internal;
            }
        }

        //TODO: del
        public static Accessibility GetExplicitAccessibility(MemberDeclarationSyntax member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            return GetAccessibility(member.GetModifiers());
        }

        public static Accessibility GetAccessibility(MemberDeclarationSyntax member)
        {
            if (member == null)
                throw new ArgumentNullException(nameof(member));

            switch (member.Kind())
            {
                case SyntaxKind.ConstructorDeclaration:
                    return GetAccessibility((ConstructorDeclarationSyntax)member);
                case SyntaxKind.MethodDeclaration:
                    return GetAccessibility((MethodDeclarationSyntax)member);
                case SyntaxKind.PropertyDeclaration:
                    return GetAccessibility((PropertyDeclarationSyntax)member);
                case SyntaxKind.IndexerDeclaration:
                    return GetAccessibility((IndexerDeclarationSyntax)member);
                case SyntaxKind.EventDeclaration:
                    return GetAccessibility((EventDeclarationSyntax)member);
                case SyntaxKind.EventFieldDeclaration:
                    return GetAccessibility((EventFieldDeclarationSyntax)member);
                case SyntaxKind.FieldDeclaration:
                    return GetAccessibility((FieldDeclarationSyntax)member);
                case SyntaxKind.ClassDeclaration:
                    return GetAccessibility((ClassDeclarationSyntax)member);
                case SyntaxKind.StructDeclaration:
                    return GetAccessibility((StructDeclarationSyntax)member);
                case SyntaxKind.InterfaceDeclaration:
                    return GetAccessibility((InterfaceDeclarationSyntax)member);
                case SyntaxKind.EnumDeclaration:
                    return GetAccessibility((EnumDeclarationSyntax)member);
                case SyntaxKind.DelegateDeclaration:
                    return GetAccessibility((DelegateDeclarationSyntax)member);
                case SyntaxKind.DestructorDeclaration:
                case SyntaxKind.OperatorDeclaration:
                case SyntaxKind.ConversionOperatorDeclaration:
                case SyntaxKind.EnumMemberDeclaration:
                case SyntaxKind.NamespaceDeclaration:
                    return Accessibility.Public;
            }

            return Accessibility.NotApplicable;
        }

        public static Accessibility GetAccessibility(ClassDeclarationSyntax classDeclaration)
        {
            if (classDeclaration == null)
                throw new ArgumentNullException(nameof(classDeclaration));

            Accessibility accessibility = GetAccessibility(classDeclaration.Modifiers);

            if (accessibility == Accessibility.NotApplicable)
                accessibility =  GetDefaultAccessibility(classDeclaration);

            return accessibility;
        }

        public static Accessibility GetAccessibility(ConstructorDeclarationSyntax constructorDeclaration)
        {
            if (constructorDeclaration == null)
                throw new ArgumentNullException(nameof(constructorDeclaration));

            SyntaxTokenList modifiers = constructorDeclaration.Modifiers;

            if (modifiers.Contains(SyntaxKind.StaticKeyword))
                return Accessibility.Private;

            Accessibility accessibility = GetAccessibility(modifiers);

            if (accessibility == Accessibility.NotApplicable)
                accessibility = GetDefaultAccessibility(constructorDeclaration);

            return accessibility;
        }

        public static Accessibility GetAccessibility(ConversionOperatorDeclarationSyntax conversionOperatorDeclaration)
        {
            if (conversionOperatorDeclaration == null)
                throw new ArgumentNullException(nameof(conversionOperatorDeclaration));

            return Accessibility.Public;
        }

        public static Accessibility GetAccessibility(DelegateDeclarationSyntax delegateDeclaration)
        {
            if (delegateDeclaration == null)
                throw new ArgumentNullException(nameof(delegateDeclaration));

            Accessibility accessibility = GetAccessibility(delegateDeclaration.Modifiers);

            if (accessibility == Accessibility.NotApplicable)
                accessibility = GetDefaultAccessibility(delegateDeclaration);

            return accessibility;
        }

        public static Accessibility GetAccessibility(DestructorDeclarationSyntax destructorDeclaration)
        {
            if (destructorDeclaration == null)
                throw new ArgumentNullException(nameof(destructorDeclaration));

            return Accessibility.Public;
        }

        public static Accessibility GetAccessibility(EnumDeclarationSyntax enumDeclaration)
        {
            if (enumDeclaration == null)
                throw new ArgumentNullException(nameof(enumDeclaration));

            Accessibility accessibility = GetAccessibility(enumDeclaration.Modifiers);

            if (accessibility == Accessibility.NotApplicable)
                accessibility = GetDefaultAccessibility(enumDeclaration);

            return accessibility;
        }

        public static Accessibility GetAccessibility(EnumMemberDeclarationSyntax enumMemberDeclaration)
        {
            if (enumMemberDeclaration == null)
                throw new ArgumentNullException(nameof(enumMemberDeclaration));

            return Accessibility.Public;
        }

        public static Accessibility GetAccessibility(EventDeclarationSyntax eventDeclaration)
        {
            if (eventDeclaration == null)
                throw new ArgumentNullException(nameof(eventDeclaration));

            if (eventDeclaration.ExplicitInterfaceSpecifier != null)
                return Accessibility.Private;

            Accessibility accessibility = GetAccessibility(eventDeclaration.Modifiers);

            if (accessibility == Accessibility.NotApplicable)
                accessibility = GetDefaultAccessibility(eventDeclaration);

            return accessibility;
        }

        public static Accessibility GetAccessibility(EventFieldDeclarationSyntax eventFieldDeclaration)
        {
            if (eventFieldDeclaration == null)
                throw new ArgumentNullException(nameof(eventFieldDeclaration));

            if (eventFieldDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
                return Accessibility.Public;

            Accessibility accessibility = GetAccessibility(eventFieldDeclaration.Modifiers);

            if (accessibility == Accessibility.NotApplicable)
                accessibility = GetDefaultAccessibility(eventFieldDeclaration);

            return accessibility;
        }

        public static Accessibility GetAccessibility(FieldDeclarationSyntax fieldDeclaration)
        {
            if (fieldDeclaration == null)
                throw new ArgumentNullException(nameof(fieldDeclaration));

            Accessibility accessibility = GetAccessibility(fieldDeclaration.Modifiers);

            if (accessibility == Accessibility.NotApplicable)
                accessibility = GetDefaultAccessibility(fieldDeclaration);

            return accessibility;
        }

        public static Accessibility GetAccessibility(IndexerDeclarationSyntax indexerDeclaration)
        {
            if (indexerDeclaration == null)
                throw new ArgumentNullException(nameof(indexerDeclaration));

            if (indexerDeclaration.ExplicitInterfaceSpecifier != null)
                return Accessibility.Private;

            if (indexerDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
                return Accessibility.Public;

            Accessibility accessibility = GetAccessibility(indexerDeclaration.Modifiers);

            if (accessibility == Accessibility.NotApplicable)
                accessibility = GetDefaultAccessibility(indexerDeclaration);

            return accessibility;
        }

        public static Accessibility GetAccessibility(InterfaceDeclarationSyntax interfaceDeclaration)
        {
            if (interfaceDeclaration == null)
                throw new ArgumentNullException(nameof(interfaceDeclaration));

            Accessibility accessibility = GetAccessibility(interfaceDeclaration.Modifiers);

            if (accessibility == Accessibility.NotApplicable)
                accessibility = GetDefaultAccessibility(interfaceDeclaration);

            return accessibility;
        }

        public static Accessibility GetAccessibility(MethodDeclarationSyntax methodDeclaration)
        {
            if (methodDeclaration == null)
                throw new ArgumentNullException(nameof(methodDeclaration));

            SyntaxTokenList modifiers = methodDeclaration.Modifiers;

            if (modifiers.Contains(SyntaxKind.PartialKeyword))
                return Accessibility.Private;

            if (methodDeclaration.ExplicitInterfaceSpecifier != null)
                return Accessibility.Private;

            if (methodDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
                return Accessibility.Public;

            Accessibility accessibility = GetAccessibility(methodDeclaration.Modifiers);

            if (accessibility == Accessibility.NotApplicable)
                accessibility = GetDefaultAccessibility(methodDeclaration);

            return accessibility;
        }

        public static Accessibility GetAccessibility(NamespaceDeclarationSyntax namespaceDeclaration)
        {
            if (namespaceDeclaration == null)
                throw new ArgumentNullException(nameof(namespaceDeclaration));

            return Accessibility.Public;
        }

        public static Accessibility GetAccessibility(OperatorDeclarationSyntax operatorDeclaration)
        {
            if (operatorDeclaration == null)
                throw new ArgumentNullException(nameof(operatorDeclaration));

            return Accessibility.Public;
        }

        public static Accessibility GetAccessibility(PropertyDeclarationSyntax propertyDeclaration)
        {
            if (propertyDeclaration == null)
                throw new ArgumentNullException(nameof(propertyDeclaration));

            if (propertyDeclaration.ExplicitInterfaceSpecifier != null)
                return Accessibility.Private;

            if (propertyDeclaration.IsParentKind(SyntaxKind.InterfaceDeclaration))
                return Accessibility.Public;

            Accessibility accessibility = GetAccessibility(propertyDeclaration.Modifiers);

            if (accessibility == Accessibility.NotApplicable)
                accessibility = GetDefaultAccessibility(propertyDeclaration);

            return accessibility;
        }

        public static Accessibility GetAccessibility(StructDeclarationSyntax structDeclaration)
        {
            if (structDeclaration == null)
                throw new ArgumentNullException(nameof(structDeclaration));

            Accessibility accessibility = GetAccessibility(structDeclaration.Modifiers);

            if (accessibility == Accessibility.NotApplicable)
                accessibility = GetDefaultAccessibility(structDeclaration);

            return accessibility;
        }

        public static Accessibility GetAccessibility(AccessorDeclarationSyntax accessorDeclaration)
        {
            if (accessorDeclaration == null)
                throw new ArgumentNullException(nameof(accessorDeclaration));

            Accessibility accessibility = GetAccessibility(accessorDeclaration.Modifiers);

            SyntaxNode declaration = accessorDeclaration.Parent?.Parent;

            if (declaration == null)
                return accessibility;

            Accessibility declarationAccessibility = GetDeclarationAccessibility();

            if (declarationAccessibility == Accessibility.NotApplicable)
                return accessibility;

            return (accessibility.IsMoreRestrictiveThan(declarationAccessibility))
                ? accessibility
                : declarationAccessibility;

            Accessibility GetDeclarationAccessibility()
            {
                switch (declaration.Kind())
                {
                    case SyntaxKind.PropertyDeclaration:
                        return GetAccessibility((PropertyDeclarationSyntax)declaration);
                    case SyntaxKind.IndexerDeclaration:
                        return GetAccessibility((IndexerDeclarationSyntax)declaration);
                    case SyntaxKind.EventDeclaration:
                        return GetAccessibility((EventDeclarationSyntax)declaration);
                }

                Debug.Fail(declaration.Kind().ToString());

                return Accessibility.NotApplicable;
            }
        }

        //TODO: GetExplicitAccessibility
        public static Accessibility GetAccessibility(SyntaxTokenList modifiers)
        {
            int count = modifiers.Count;

            for (int i = 0; i < count; i++)
            {
                switch (modifiers[i].Kind())
                {
                    case SyntaxKind.PublicKeyword:
                        {
                            return Accessibility.Public;
                        }
                    case SyntaxKind.PrivateKeyword:
                        {
                            for (int j = i + 1; j < count; j++)
                            {
                                if (modifiers[j].Kind() == SyntaxKind.ProtectedKeyword)
                                    return Accessibility.ProtectedAndInternal;
                            }

                            return Accessibility.Private;
                        }
                    case SyntaxKind.InternalKeyword:
                        {
                            for (int j = i + 1; j < count; j++)
                            {
                                if (modifiers[j].Kind() == SyntaxKind.ProtectedKeyword)
                                    return Accessibility.ProtectedOrInternal;
                            }

                            return Accessibility.Internal;
                        }
                    case SyntaxKind.ProtectedKeyword:
                        {
                            for (int j = i + 1; j < count; j++)
                            {
                                switch (modifiers[j].Kind())
                                {
                                    case SyntaxKind.InternalKeyword:
                                        return Accessibility.ProtectedOrInternal;
                                    case SyntaxKind.PrivateKeyword:
                                        return Accessibility.ProtectedAndInternal;
                                }
                            }

                            return Accessibility.Protected;
                        }
                }
            }

            return Accessibility.NotApplicable;
        }

        public static TNode ChangeAccessibility<TNode>(
            TNode node,
            Accessibility newAccessibility,
            IModifierComparer comparer = null) where TNode : SyntaxNode
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            AccessibilityInfo info = SyntaxInfo.AccessibilityInfo(node);

            AccessibilityInfo newInfo = ChangeAccessibility(info, newAccessibility, comparer);

            return (TNode)newInfo.Declaration;
        }

        public static AccessibilityInfo ChangeAccessibility(
            AccessibilityInfo info,
            Accessibility newAccessibility,
            IModifierComparer comparer = null)
        {
            if (!info.Success)
                return info;

            Accessibility accessibility = info.Accessibility;

            if (accessibility == newAccessibility)
                return info;

            comparer = comparer ?? ModifierComparer.Instance;

            SyntaxNode declaration = info.Declaration;

            if (accessibility.IsSingleTokenAccessibility()
                && newAccessibility.IsSingleTokenAccessibility())
            {
                int insertIndex = comparer.GetInsertIndex(info.Modifiers, GetTokenKind(newAccessibility));

                if (info.TokenIndex == insertIndex
                    || info.TokenIndex == insertIndex - 1)
                {
                    SyntaxToken newToken = SyntaxFactory.Token(GetTokenKind(newAccessibility)).WithTriviaFrom(info.Token);

                    SyntaxTokenList newModifiers = info.Modifiers.Replace(info.Token, newToken);

                    return info.WithModifiers(newModifiers);
                }
            }

            if (accessibility != Accessibility.NotApplicable)
            {
                declaration = Modifier.RemoveAt(declaration, Math.Max(info.TokenIndex, info.SecondTokenIndex));

                if (info.SecondTokenIndex != -1)
                    declaration = Modifier.RemoveAt(declaration, Math.Min(info.TokenIndex, info.SecondTokenIndex));
            }

            if (newAccessibility != Accessibility.NotApplicable)
            {
                declaration = InsertModifier(declaration, newAccessibility, comparer);
            }

            return SyntaxInfo.AccessibilityInfo(declaration);
        }

        private static SyntaxNode InsertModifier(SyntaxNode node, Accessibility accessibility, IModifierComparer comparer)
        {
            switch (accessibility)
            {
                case Accessibility.Private:
                    {
                        return node.InsertModifier(SyntaxKind.PrivateKeyword, comparer);
                    }
                case Accessibility.Protected:
                    {
                        return node.InsertModifier(SyntaxKind.ProtectedKeyword, comparer);
                    }
                case Accessibility.ProtectedAndInternal:
                    {
                        node = node.InsertModifier(SyntaxKind.PrivateKeyword, comparer);

                        return node.InsertModifier(SyntaxKind.ProtectedKeyword, comparer);
                    }
                case Accessibility.Internal:
                    {
                        return node.InsertModifier(SyntaxKind.InternalKeyword, comparer);
                    }
                case Accessibility.Public:
                    {
                        return node.InsertModifier(SyntaxKind.PublicKeyword, comparer);
                    }
                case Accessibility.ProtectedOrInternal:
                    {
                        node = node.InsertModifier(SyntaxKind.ProtectedKeyword, comparer);

                        return node.InsertModifier(SyntaxKind.InternalKeyword, comparer);
                    }
            }

            return node;
        }

        private static SyntaxKind GetTokenKind(Accessibility accessibility)
        {
            switch (accessibility)
            {
                case Accessibility.Private:
                    return SyntaxKind.PrivateKeyword;
                case Accessibility.Protected:
                    return SyntaxKind.ProtectedKeyword;
                case Accessibility.Internal:
                    return SyntaxKind.InternalKeyword;
                case Accessibility.Public:
                    return SyntaxKind.PublicKeyword;
                case Accessibility.NotApplicable:
                    return SyntaxKind.None;
                default:
                    throw new ArgumentException("", nameof(accessibility));
            }
        }

        public static bool IsAllowedAccessibility(SyntaxNode node, Accessibility accessibility, bool ignoreOverride = false)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            switch (node.Parent?.Kind())
            {
                case SyntaxKind.NamespaceDeclaration:
                case SyntaxKind.CompilationUnit:
                    {
                        return accessibility.Is(Accessibility.Public, Accessibility.Internal);
                    }
                case SyntaxKind.StructDeclaration:
                    {
                        if (accessibility.ContainsProtected())
                            return false;

                        break;
                    }
            }

            switch (node.Kind())
            {
                case SyntaxKind.ClassDeclaration:
                case SyntaxKind.InterfaceDeclaration:
                case SyntaxKind.StructDeclaration:
                case SyntaxKind.EnumDeclaration:
                    {
                        return true;
                    }
                case SyntaxKind.EventDeclaration:
                    {
                        var eventDeclaration = (EventDeclarationSyntax)node;

                        ModifiersInfo info = SyntaxInfo.ModifiersInfo(eventDeclaration);

                        return (ignoreOverride || !info.HasOverride)
                            && (!accessibility.IsPrivate() || !info.HasAbstractOrVirtualOrOverride)
                            && CheckProtectedInStaticOrSealedClass(node, accessibility)
                            && CheckAccessorAccessibility(eventDeclaration.AccessorList, accessibility);
                    }
                case SyntaxKind.IndexerDeclaration:
                    {
                        var indexerDeclaration = (IndexerDeclarationSyntax)node;

                        ModifiersInfo info = SyntaxInfo.ModifiersInfo(indexerDeclaration);

                        return (ignoreOverride || !info.HasOverride)
                            && (!accessibility.IsPrivate() || !info.HasAbstractOrVirtualOrOverride)
                            && CheckProtectedInStaticOrSealedClass(node, accessibility)
                            && CheckAccessorAccessibility(indexerDeclaration.AccessorList, accessibility);
                    }
                case SyntaxKind.PropertyDeclaration:
                    {
                        var propertyDeclaration = (PropertyDeclarationSyntax)node;

                        ModifiersInfo info = SyntaxInfo.ModifiersInfo(propertyDeclaration);

                        return (ignoreOverride || !info.HasOverride)
                            && (!accessibility.IsPrivate() || !info.HasAbstractOrVirtualOrOverride)
                            && CheckProtectedInStaticOrSealedClass(node, accessibility)
                            && CheckAccessorAccessibility(propertyDeclaration.AccessorList, accessibility);
                    }
                case SyntaxKind.MethodDeclaration:
                    {
                        var methodDeclaration = (MethodDeclarationSyntax)node;

                        ModifiersInfo info = SyntaxInfo.ModifiersInfo(methodDeclaration);

                        return (ignoreOverride || !info.HasOverride)
                            && (!accessibility.IsPrivate() || !info.HasAbstractOrVirtualOrOverride)
                            && CheckProtectedInStaticOrSealedClass(node, accessibility);
                    }
                case SyntaxKind.EventFieldDeclaration:
                    {
                        var eventFieldDeclaration = (EventFieldDeclarationSyntax)node;

                        ModifiersInfo info = SyntaxInfo.ModifiersInfo(eventFieldDeclaration);

                        return (ignoreOverride || !info.HasOverride)
                            && (!accessibility.IsPrivate() || !info.HasAbstractOrVirtualOrOverride)
                            && CheckProtectedInStaticOrSealedClass(node, accessibility);
                    }
                case SyntaxKind.ConstructorDeclaration:
                case SyntaxKind.DelegateDeclaration:
                case SyntaxKind.FieldDeclaration:
                case SyntaxKind.IncompleteMember:
                    {
                        return CheckProtectedInStaticOrSealedClass(node, accessibility);
                    }
                case SyntaxKind.OperatorDeclaration:
                case SyntaxKind.ConversionOperatorDeclaration:
                    {
                        return accessibility == Accessibility.Public;
                    }
                case SyntaxKind.GetAccessorDeclaration:
                case SyntaxKind.SetAccessorDeclaration:
                case SyntaxKind.AddAccessorDeclaration:
                case SyntaxKind.RemoveAccessorDeclaration:
                case SyntaxKind.UnknownAccessorDeclaration:
                    {
                        var memberDeclaration = node.Parent?.Parent as MemberDeclarationSyntax;

                        Debug.Assert(memberDeclaration != null, node.ToString());

                        if (memberDeclaration != null)
                        {
                            if (!CheckProtectedInStaticOrSealedClass(memberDeclaration, accessibility))
                                return false;

                            return accessibility.IsMoreRestrictiveThan(GetAccessibility(memberDeclaration));
                        }

                        return false;
                    }
                case SyntaxKind.LocalFunctionStatement:
                    {
                        return false;
                    }
                default:
                    {
                        Debug.Fail(node.Kind().ToString());
                        return false;
                    }
            }
        }

        private static bool CheckProtectedInStaticOrSealedClass(SyntaxNode node, Accessibility accessibility)
        {
            return !accessibility.ContainsProtected()
                || (node.Parent as ClassDeclarationSyntax)?
                    .Modifiers
                    .ContainsAny(SyntaxKind.StaticKeyword, SyntaxKind.SealedKeyword) != true;
        }

        private static bool CheckAccessorAccessibility(AccessorListSyntax accessorList, Accessibility accessibility)
        {
            if (accessorList != null)
            {
                foreach (AccessorDeclarationSyntax accessor in accessorList.Accessors)
                {
                    Accessibility accessorAccessibility = GetAccessibility(accessor.Modifiers);

                    if (accessorAccessibility != Accessibility.NotApplicable)
                        return accessorAccessibility.IsMoreRestrictiveThan(accessibility);
                }
            }

            return true;
        }
    }
}