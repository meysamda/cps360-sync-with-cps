namespace CPS360.Sync.CSD.Application.Common.DomainExceptions
{
    public enum ErrorMessage
    {
        // bad request (400 series) client error details
        entityWithTheSameKeyAlreadyExists,

        // unauthorized (401 series) client error details

        // forbidden (403 series) client error details

        // not found (404 series) client error details
        notFound,

        // conflict (409 series) client error details
    }
}