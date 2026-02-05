using DataAccessLayer.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BusinessLayer.Helpers
{
    public static class FileHelpers
    {
        /// <summary>
        /// Creates a list of FileUpload from inputted IFormFiles, with accompanying workRequestId and username.
        /// Doesn't check for duplicate file hashes or filenames.
        /// </summary>
        /// <param name="workRequestId">Id of the work request, same as in the database.</param>
        /// <param name="formFiles">Files to be uploaded.</param>
        /// <param name="username">Username of uploader.</param>
        /// <returns></returns>
        public static List<FileUpload> PopulateFileUpload(int workRequestId, IEnumerable<IFormFile> formFiles, string username)
        {
            return PopulateFileUpload(workRequestId, formFiles, username, failedUploads: out _, existingFiles: null);
        }


        /// <summary>
        /// Creates a list of FileUpload from inputted IFormFiles, with accompanying workRequestId and username.
        /// Also checks against <paramref name="existingFiles"/> for duplicate files and filenames. Skips duplicate files, renames duplicate names in form "filename(i).ext".
        /// </summary>
        /// <param name="workRequestId">Id of the work request, same as in the database.</param>
        /// <param name="formFiles">Files to be uploaded.</param>
        /// <param name="username">Username of uploader.</param>
        /// <param name="existingFiles">Existing files to compare name and hashes against for duplicates.</param>
        /// <returns></returns>
        public static List<FileUpload> PopulateFileUpload(int workRequestId, IEnumerable<IFormFile> formFiles, string username, out IEnumerable<string> failedUploads, IEnumerable<FileUpload> existingFiles = null)
        {
            failedUploads = null;

            // If inputs are invalid or there are no formFiles, quit out
            if (workRequestId == 0 || formFiles == null || !formFiles.Any() || string.IsNullOrWhiteSpace(username))
                return null;

            List<FileUpload> files = new();

            foreach (IFormFile formFile in formFiles)
            {
                if (formFile.Length > 0)
                {
                    // Getting a readable filename.
                    string initialFilename = Path.GetFileName(formFile.FileName);
                    string readableFileName = initialFilename.GetNextFilename(existingFiles);
                    // Getting file Extension
                    string fileExtension = Path.GetExtension(readableFileName);
                    // Concatenating  FileName + FileExtension
                    string newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), readableFileName);
                    // Extract file bytes
                    byte[] file = null;
                    string hash = "";
                    using (MemoryStream ms = new())
                    {
                        formFile.CopyTo(ms);
                        file = ms.ToArray();
                    }
                    // Compute file hash
                    using (SHA256 sha256Hash = SHA256.Create())
                    {
                        hash = Convert.ToBase64String(sha256Hash.ComputeHash(file));
                    }

                    // Check for duplicate file uploads. If already uploaded against this request, skip!
                    if (existingFiles != null && existingFiles.Any(x => x.FileHash == hash))
                    {
                        if (failedUploads == null)
                            failedUploads = new List<string>() { initialFilename };
                        else
                            _ = failedUploads.Append(initialFilename);
                        continue;
                    }

                    // Create FileUpload object and add to list to be returned.
                    FileUpload fileUpload = new()
                    {
                        WorkRequestId = workRequestId,
                        FileName = newFileName,
                        ReadableFileName = readableFileName,
                        FileUploadDateTime = DateTime.Now,
                        FileHash = hash,
                        File = file,
                        UploadedBy = username,
                    };
                    files.Add(fileUpload);
                }
            }
            return files;
        }


        /// <summary>
        /// Gets the next unique filename to be added to a set of existing files.
        /// E.g. Penguin.png, Penguin(2).png, Penguin(3).png
        /// </summary>
        /// <param name="filename">Filename including extension</param>
        /// <param name="existingFiles">Existing files to compare names against. If none, just returns <paramref name="filename"/></param>
        /// <returns></returns>
        private static string GetNextFilename(this string filename, IEnumerable<FileUpload> existingFiles = null)
        {
            if (existingFiles == null || !existingFiles.Any())
                return filename;

            int i = 1; // NB: files start incrementing from 2 even though 1 here.
            string file = Path.GetFileNameWithoutExtension(filename);
            string extension = Path.GetExtension(filename);

            while (existingFiles.Any(x => x.ReadableFileName == filename))
            {
                i++; // Causes start at i = 2, then +1 onwards.
                filename = $"{file}({i}){extension}";
            }

            return filename;
        }

        public static string GetTimeStamp()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }

       

    }
}
