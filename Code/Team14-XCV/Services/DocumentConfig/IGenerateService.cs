using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace XCV.Data
{
    public interface IGenerateService
    {
        ///=======================================================
        ///======= Methods for generating a Word-Document: =======
        ///=======================================================

        /// Summary:
        /// Input can be adjusted to anything, must only adjust DocumentFormat.OpenXml accordingly (for Future additions/purposes, easy to add))
        /// Opens a template.docx worddocument, adds information of Employee e to the.docx and returns the combined .docx in a byte-stream.

        /// <summary>
        /// Generates a Worddocument with the content of a single Employee profile
        /// </summary>
        /// <param name="e"></param>
        /// <returns>A byte-Array with the document</returns>
        public byte[] GenerateSingleProfile(Employee e);

        /// <summary>
        /// Uses the DocumentConfig associated to the current offer to generater the offer's document. 
        /// <para>Expects o and config to carry the data.</para>
        /// </summary>
        /// <param name="o"></param>
        /// <param name="config"></param>
        /// <returns>A byte-Array with the document</returns>
        public byte[] GenerateDocumentConfig(Offer o, DocumentConfig config);

        /// <summary>
        /// Blueprint which can be modified to output anything.
        /// </summary>
        /// <param name="o"></param>
        /// <returns>A byte-Array with the document</returns>
        public byte[] Anything(Object o);

        /// Additions:

        /// <summary>
        /// Adds a Page break to a referenced body
        /// </summary>
        /// <param name="body"></param>
        public void AddPageBreak(Body body);

        /// <summary>
        /// Adds an image to a referenced body
        /// </summary>
        /// <param name="body"></param>
        /// <param name="relationshipId"></param>
        public void AddImageToBody(Body body, string relationshipId);
    }
}
