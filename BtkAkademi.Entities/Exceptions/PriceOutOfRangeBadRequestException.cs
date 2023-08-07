

namespace BtkAkademi.Entities.Exceptions
{
    public class PriceOutOfRangeBadRequestException : BadRequestException
    {
        public PriceOutOfRangeBadRequestException() : base("Minimum Price must be greater than 10 and less than 1000")
        {

        }
    }
}
