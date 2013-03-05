﻿/* Copyright 2010-2013 10gen Inc.
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
* http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/

using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

namespace MongoDB.Driver.GeoJsonObjectModel.Serializers
{
    public class GeoJsonGeometryCollectionSerializer<TCoordinates> : GeoJsonGeometrySerializer<TCoordinates> where TCoordinates : GeoJsonCoordinates
    {
        // private fields
        private readonly IBsonSerializer _geometrySerializer = BsonSerializer.LookupSerializer(typeof(GeoJsonGeometry<TCoordinates>));

        // public methods
        public override object Deserialize(BsonReader bsonReader, Type nominalType, Type actualType, IBsonSerializationOptions options)
        {
            return DeserializeGeoJsonObject(bsonReader, new GeometryCollectionData());
        }

        public override void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
        {
            SerializeGeoJsonObject(bsonWriter, (GeoJsonObject<TCoordinates>)value);
        }

        // protected methods
        protected override void DeserializeField(BsonReader bsonReader, string name, ObjectData data)
        {
            var geometryCollectionData = (GeometryCollectionData)data;
            switch (name)
            {
                case "geometries": geometryCollectionData.Geometries = DeserializeGeometries(bsonReader); break;
                default: base.DeserializeField(bsonReader, name, data); break;
            }
        }

        protected override void SerializeFields(BsonWriter bsonWriter, GeoJsonObject<TCoordinates> obj)
        {
            var geometryCollection = (GeoJsonGeometryCollection<TCoordinates>)obj;
            SerializeGeometries(bsonWriter, geometryCollection.Geometries);
        }

        // private methods
        private List<GeoJsonGeometry<TCoordinates>> DeserializeGeometries(BsonReader bsonReader)
        {
            var geometries = new List<GeoJsonGeometry<TCoordinates>>();

            bsonReader.ReadStartArray();
            while (bsonReader.ReadBsonType() != BsonType.EndOfDocument)
            {
                var geometry = (GeoJsonGeometry<TCoordinates>)_geometrySerializer.Deserialize(bsonReader, typeof(GeoJsonGeometry<TCoordinates>), null);
                geometries.Add(geometry);
            }
            bsonReader.ReadEndArray();

            return geometries;
        }

        private void SerializeGeometries(BsonWriter bsonWriter, IEnumerable<GeoJsonGeometry<TCoordinates>> geometries)
        {
            bsonWriter.WriteName("geometries");
            bsonWriter.WriteStartArray();
            foreach (var geometry in geometries)
            {
                _geometrySerializer.Serialize(bsonWriter, typeof(GeoJsonGeometry<TCoordinates>), geometry, null);
            }
            bsonWriter.WriteEndArray();
        }

        // nested classes
        private class GeometryCollectionData : ObjectData
        {
            // private fields
            private List<GeoJsonGeometry<TCoordinates>> _geometries;

            // constructors
            public GeometryCollectionData()
                : base("GeometryCollection")
            {
            }

            // public properties
            public List<GeoJsonGeometry<TCoordinates>> Geometries
            {
                get { return _geometries; }
                set { _geometries = value; }
            }

            // public methods
            public override object CreateInstance()
            {
                return new GeoJsonGeometryCollection<TCoordinates>(Args, _geometries);
            }
        }
    }
}
