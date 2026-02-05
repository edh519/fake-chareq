using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BusinessLayer.ViewModels
{
    public class SupportingFilesTableViewModel
    {
        public SupportingFilesTableViewModel(IEnumerable<FileUpload> fileUploads)
        {
            List<SupportingFileTableViewModel> supportingFileTableViewModels = new List<SupportingFileTableViewModel>();
            if (fileUploads != null)
            {
                foreach (var fileUpload in fileUploads)
                {
                    supportingFileTableViewModels.Add(new SupportingFileTableViewModel(fileUpload));
                }
            }
            FileUploads = supportingFileTableViewModels;

        }


        public List<SupportingFileTableViewModel> FileUploads { get; set; }

        public class SupportingFileTableViewModel
        {
            public SupportingFileTableViewModel(FileUpload fileUpload)
            {
                FileUploadId = fileUpload.FileUploadId;
                if (string.IsNullOrWhiteSpace(fileUpload.ReadableFileName))
                {
                    FileName = fileUpload.FileName;
                }
                else
                {
                    FileName = fileUpload.ReadableFileName;
                }
                FileUploadDateTime = fileUpload.FileUploadDateTime;
                UploadedBy = fileUpload.UploadedBy;
            }

            public int FileUploadId { get; set; }

            [DisplayName("File Name")]
            public string FileName { get; set; }

            [DisplayName("Uploaded at")]
            public DateTime FileUploadDateTime { get; set; }

            [DisplayName("Uploaded by")]
            public string UploadedBy { get; set; }
        }

    }
}
