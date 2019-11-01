using AutoMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sparrow.Core.Mapping.Converters
{
    public class FormFileConverter : IValueConverter<IFormFile, byte[]>
    {
        public byte[] Convert(IFormFile src, ResolutionContext context)
        {
            if (src == null) return null;

            using (var ms = new MemoryStream())
            {
                src.CopyTo(ms);
                ms.Position = 0;

                var bytes = new List<byte>();
                int readed;
                var buffer = new byte[10240];

                while ((readed = ms.Read(buffer, 0, buffer.Length)) > 0)
                {
                    bytes.AddRange(buffer.AsSpan().Slice(0, readed).ToArray());
                }

                return bytes.ToArray();
            }
        }
    }
}
