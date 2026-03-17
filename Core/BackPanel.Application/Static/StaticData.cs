using DinkToPdf;
using DinkToPdf.Contracts;
using BackPanel.Application.DTOs;

namespace BackPanel.Application.Static
{
    public static class StaticData
    {
        public static BusinessDto? Business { get; set; }
        public static IConverter Converter { get; set; } = new SynchronizedConverter(new PdfTools());
    }
}