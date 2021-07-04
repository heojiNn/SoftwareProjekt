namespace XCV.Data
{
    public interface IConfigService
    {
        //TODO: Defines the Methods for generating:

        // Offer-Document: Possible Methods:
        // selected/deselect/delete/create/update//clear/undo/redo/reset/copy/rename/download

        // Document of Employee Detailview: Possible Methods:
        // selected/deselect/delete/create/update//clear/undo/redo/reset/copy/rename/download

        // ...


        // Input can be anything, must only adjust DocumentFormat.OpenXml accordingly (for Future additions/purposes, easy to add))


        /// <summary>
        /// Opens a template .docx worddocument, adds information of Employee e to the .docx and returns the combined .docx in a byte-stream.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public byte[] GenerateSingleProfile(Employee e);

        public byte[] GenerateDocumentConfig(DocumentConfig config);














    }
}
