using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BackPanel.Persistence.Database.Extensions;

public static class DbUpdateExceptionDecoder
{
    public static string Decode(this DbUpdateException exception)
    {
        if (exception.InnerException is SqlException sqlException)
        {
            switch (sqlException.Number)
            {
                case 2601:
                    return "This Item is Already exist in database";
                default:
                    return "Error Occurred While updating the database !";
            }

        }
        else
        {
            return "Error Occurred While updating the database !";
        }
    }
}