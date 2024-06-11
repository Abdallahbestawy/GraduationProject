using Newtonsoft.Json;

namespace GraduationProject.LogHandler.Service
{
    public static class ObjectDuplicater
    {
        public static T Duplicate<T>(T source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source), "Source object cannot be null");
            }

            // Serialize the object to JSON
            var serializedObject = JsonConvert.SerializeObject(source);

            // Deserialize the JSON back into an object of type T
            var clonedObject = JsonConvert.DeserializeObject<T>(serializedObject);

            return clonedObject;
        }
    }
}
