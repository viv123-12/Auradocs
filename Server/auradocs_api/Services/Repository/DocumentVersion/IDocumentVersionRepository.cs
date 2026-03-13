public interface IDocumentVersionRepository
{
    public Task<List<DocumentVersion>> GetDocumentVersionAsync(string documenId);
    public Task AddDocumentVersionAsync(DocumentVersion documentVersion);
}