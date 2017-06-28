﻿// Copyright (c) Josef Pihrt. All rights reserved. Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Roslynator.CSharp.CodeFixes.Test
{
    internal static class CreateSingletonArray
    {
        private static void Foo(string[] values)
        {
            string s = null;
            object o = null;

            object[] arr = o;

            Foo(s);
        }
    }
}
