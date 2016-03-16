﻿namespace Snippets5.Serialization
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Bson;
    using Newtonsoft.Json.Converters;
    using NServiceBus;
    using NServiceBus.Newtonsoft.Json;
    using NServiceBus.Serialization;

    public class NewtonsoftSerializerUsage
    {
        public void Simple()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region NewtonsoftSerialization [0.2,0.3)

            busConfiguration.UseSerialization<NewtonsoftSerializer>();

            #endregion
        }

        public void CustomSettings()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region NewtonsoftCustomSettings [0.2,0.3)

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
                Converters =
                {
                    new IsoDateTimeConverter
                    {
                        DateTimeStyles = DateTimeStyles.RoundtripKind
                    }
                }
            };
            busConfiguration.UseSerialization<NewtonsoftSerializer>()
                .Settings(settings);

            #endregion
        }

        public void CustomReader()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region NewtonsoftCustomReader [0.2,0.3)

            busConfiguration.UseSerialization<NewtonsoftSerializer>()
                .ReaderCreator(stream =>
                {
                    StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
                    return new JsonTextReader(streamReader);
                });

            #endregion
        }

        public void CustomWriter()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region NewtonsoftCustomWriter [0.2,0.3)

            busConfiguration.UseSerialization<NewtonsoftSerializer>()
                .WriterCreator(stream =>
                {
                    StreamWriter streamWriter = new StreamWriter(stream, Encoding.UTF8);
                    return new JsonTextWriter(streamWriter)
                    {
                        Formatting = Formatting.None
                    };
                });

            #endregion
        }
        public void Bson()
        {
            BusConfiguration busConfiguration = new BusConfiguration();
            #region NewtonsoftBson [0.2,0.3)

            SerializationExtentions<NewtonsoftSerializer> serialization =
                busConfiguration.UseSerialization<NewtonsoftSerializer>();
            serialization.ReaderCreator(stream => new BsonReader(stream));
            serialization.WriterCreator(stream => new BsonWriter(stream));

            #endregion
        }

        #region NewtonsoftAttributes [0.2,0.3)

        [JsonObject(MemberSerialization.OptIn)]
        public class CreatePersonMessage : IMessage
        {
            // "John Smith"
            [JsonProperty]
            public string Name { get; set; }

            // "2000-12-15T22:11:03"
            [JsonProperty]
            public DateTime BirthDate { get; set; }

            // new Date(976918263055)
            [JsonProperty]
            [JsonConverter(typeof(JavaScriptDateTimeConverter))]
            public DateTime LastModified { get; set; }

            // not serialized because mode is opt-in
            public string Department { get; set; }
        }

        #endregion
    }
}