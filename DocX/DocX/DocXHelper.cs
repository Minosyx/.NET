using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml;

namespace DocX
{
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Wordprocessing;
    using static FormatDocX;
    public static class FormatDocX
    {
        private const int DefaultFontSize = 21;
        private const int HeaderFontSize = 48;
        private const string FontFamily = "Arial";

        public static Color HighlightColor => new Color() {Val = "000055"};
        public static Color DefaultColor => new Color() {Val = "Black"};

        public static RunFonts DefaultFont => new RunFonts() {Ascii = FontFamily, HighAnsi = FontFamily};

        public static RunProperties DefaultRunStyle => new RunProperties()
        {
            RunFonts = DefaultFont,
            FontSize = new FontSize()
            {
                Val = new StringValue(DefaultFontSize.ToString())
            },
            Color = DefaultColor
        };

        public static RunProperties DefaultTitleStyle => new RunProperties()
        {
            Bold = new Bold()
            {
                Val = OnOffValue.FromBoolean(true)
            },
            RunFonts = DefaultFont,
            FontSize = new FontSize()
            {
                Val = new StringValue(HeaderFontSize.ToString())
            },
            Color = HighlightColor
        };

        public static ParagraphProperties ParagraphInterlineExpand
        {
            get
            {
                ParagraphProperties paragraphProperties = new ParagraphProperties();
                paragraphProperties.Append(new SpacingBetweenLines() {Line = "360"});
                return paragraphProperties;
            }
        }
}
    public static class DocXHelper
    {
        private static Paragraph createParagraph(string[] lines, int fontSize, Color fontColor, bool interline = false)
        {
            Paragraph paragraph = new Paragraph();
            if (!interline) paragraph.Append(ParagraphInterlineExpand);
            Run run = paragraph.AppendChild(new Run());
            RunProperties runStyle = DefaultRunStyle;
            runStyle.FontSize.Val = new StringValue(fontSize.ToString());
            runStyle.Color = fontColor;
            run.Append(runStyle);
            foreach (string line in lines)
            {
                run.AppendChild(new Text(line));
                run.AppendChild(new Break());
            }
            return paragraph;
        }

        private static Paragraph createTitleParagraph(string title)
        {
            Paragraph titleParagraph = new Paragraph();
            Run run = titleParagraph.AppendChild(new Run());
            run.Append(DefaultTitleStyle);
            run.Append(new Text(title));
            return titleParagraph;
        }

        private static Stream obtainDrawing(string filePath)
        {
            Stream stream = new FileStream(filePath, FileMode.Open);
            return stream;
        }

        private static Drawing createDrawing(WordprocessingDocument wordDocument, Stream stream)
        {
            MainDocumentPart mainDocumentPart = wordDocument.MainDocumentPart;
            ImagePart drawingPart = mainDocumentPart.AddImagePart(ImagePartType.Png);
            drawingPart.FeedData(stream);
            return DrawingDocX.CreateDrawingElement(wordDocument, mainDocumentPart.GetIdOfPart(drawingPart), 3, 1.5f);
        }

        private static void sendDocumentToStream(Raport raport, Stream stream, IFormatProvider formatProvider)
        {
            using (WordprocessingDocument wordDocument = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document, true))
            {
                MainDocumentPart mainDocumentPart = wordDocument.AddMainDocumentPart();
                mainDocumentPart.Document = new Document();

                Body documentBody = new Body();
                mainDocumentPart.Document.AppendChild(documentBody);



                string[] headerLines =
                {
                    "e-mail: jacek@fizyka.umk.pl",
                    "www.umk.pl",
                    "tel.: +48 56 61111 3318"
                };
                Paragraph titleParagraph = createParagraph(headerLines, 18, HighlightColor, true);
                documentBody.AppendChild(titleParagraph);

                documentBody.AppendChild(createTitleParagraph("Raport"));
                //paragraph
                Paragraph paragraph = createParagraph(raport.Lines, 25, DefaultColor);
                documentBody.AppendChild(paragraph);
                wordDocument.Close();
            }
            stream.Flush();
        }

        public static void DocXExport(this Raport raport, string filePath, IFormatProvider? formatProvider = null)
        {
            if (formatProvider == null) formatProvider = new CultureInfo("pl-PL");
            if (File.Exists(filePath)) File.Delete(filePath);
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                sendDocumentToStream(raport, fs, formatProvider);
                fs.Close();
            }
        }
    }
}
