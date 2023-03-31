using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BackPanel.Persistence.Database.Extensions;

public static class DbUpdateExceptionDecoder
{
    public static string Decode(this DbUpdateException exception)
    {
        if (exception.InnerException is SqlException sqlException)
        {
            return sqlException.Number switch
            {
                2601 => "This Item is Already exist in database",
                _ => "Error Occurred While updating the database !",
            };
        }
        else
        {
            return "Error Occurred While updating the database !";
        }
    }
}