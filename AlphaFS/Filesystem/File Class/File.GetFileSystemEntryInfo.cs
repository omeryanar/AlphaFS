/*  Copyright (C) 2008-2017 Peter Palotas, Jeffrey Jangli, Alexandr Normuradov
 *  
 *  Permission is hereby granted, free of charge, to any person obtaining a copy 
 *  of this software and associated documentation files (the "Software"), to deal 
 *  in the Software without restriction, including without limitation the rights 
 *  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell 
 *  copies of the Software, and to permit persons to whom the Software is 
 *  furnished to do so, subject to the following conditions:
 *  
 *  The above copyright notice and this permission notice shall be included in 
 *  all copies or substantial portions of the Software.
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE 
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER 
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, 
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN 
 *  THE SOFTWARE. 
 */

using System;
using System.Security;

namespace Alphaleonis.Win32.Filesystem
{
   public static partial class File
   {
      /// <summary>[AlphaFS] Gets the <see cref="FileSystemEntryInfo"/> of the file on the path.</summary>
      /// <returns>The <see cref="FileSystemEntryInfo"/> instance of the file.</returns>
      /// <param name="path">The path to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfo(string path, PathFormat pathFormat)
      {
         return GetFileSystemEntryInfoCore(false, null, path, false, pathFormat);
      }


      /// <summary>[AlphaFS] Gets the <see cref="FileSystemEntryInfo"/> of the file on the path.</summary>
      /// <returns>The <see cref="FileSystemEntryInfo"/> instance of the file.</returns>
      /// <param name="path">The path to the file.</param>
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfo(string path)
      {
         return GetFileSystemEntryInfoCore(false, null, path, false, PathFormat.RelativePath);
      }

      /// <summary>[AlphaFS] Gets the <see cref="FileSystemEntryInfo"/> of the file on the path.</summary>
      /// <returns>The <see cref="FileSystemEntryInfo"/> instance of the file.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file.</param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfoTransacted(KernelTransaction transaction, string path, PathFormat pathFormat)
      {
         return GetFileSystemEntryInfoCore(false, transaction, path, false, pathFormat);
      }


      /// <summary>[AlphaFS] Gets the <see cref="FileSystemEntryInfo"/> of the file on the path.</summary>
      /// <returns>The <see cref="FileSystemEntryInfo"/> instance of the file.</returns>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file.</param>
      [SecurityCritical]
      public static FileSystemEntryInfo GetFileSystemEntryInfoTransacted(KernelTransaction transaction, string path)
      {
         return GetFileSystemEntryInfoCore(false, transaction, path, false, PathFormat.RelativePath);
      }


      /// <summary>Gets a FileSystemEntryInfo from a Non-/Transacted file or directory.</summary>
      /// <returns>The <see cref="FileSystemEntryInfo"/> instance of the file or directory, or <c>null</c> on Exception when <paramref name="continueOnException"/> is <c>true</c>.</returns>
      /// <remarks>BasicSearch <see cref="NativeMethods.FINDEX_INFO_LEVELS.Basic"/> and LargeCache <see cref="NativeMethods.FIND_FIRST_EX_AdditionalFlags.LargeFetch"/> are used by default, if possible.</remarks>
      /// <exception cref="ArgumentException"/>
      /// <exception cref="ArgumentNullException"/>
      /// <param name="isFolder">Specifies that <paramref name="path"/> is a file or directory.</param>
      /// <param name="transaction">The transaction.</param>
      /// <param name="path">The path to the file.</param>
      /// <param name="continueOnException">
      ///    <para><c>true</c> suppress any Exception that might be thrown as a result from a failure,</para>
      ///    <para>such as ACLs protected directories or non-accessible reparse points.</para>
      /// </param>
      /// <param name="pathFormat">Indicates the format of the path parameter(s).</param>
      [SecurityCritical]
      internal static FileSystemEntryInfo GetFileSystemEntryInfoCore(bool isFolder, KernelTransaction transaction, string path, bool continueOnException, PathFormat pathFormat)
      {
         // Enable BasicSearch and LargeCache by default.
         var options = DirectoryEnumerationOptions.BasicSearch | DirectoryEnumerationOptions.LargeCache | (continueOnException ? DirectoryEnumerationOptions.ContinueOnException : 0);

         return new FindFileSystemEntryInfo(isFolder, transaction, path, Path.WildcardStarMatchAll, options, typeof(FileSystemEntryInfo), pathFormat).Get<FileSystemEntryInfo>();
      }
   }
}
