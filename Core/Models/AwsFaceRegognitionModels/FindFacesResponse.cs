namespace Core.Models.AwsFaceRegognitionModels
{
    public class FindFacesResponse
    {
        public FindFacesResponse(string fileName)
        {
            DrawnImage = fileName;
        }

        public string DrawnImage { get; private set; }
    }
}
