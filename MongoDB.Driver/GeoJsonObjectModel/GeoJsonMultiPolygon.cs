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
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel.Serializers;

namespace MongoDB.Driver.GeoJsonObjectModel
{
    [BsonSerializer(typeof(GeoJsonMultiPolygonSerializer<>))]
    public class GeoJsonMultiPolygon<TCoordinates> : GeoJsonGeometry<TCoordinates> where TCoordinates : GeoJsonCoordinates
    {
        // private fields
        private GeoJsonMultiPolygonCoordinates<TCoordinates> _coordinates;

        // constructors
        public GeoJsonMultiPolygon(GeoJsonMultiPolygonCoordinates<TCoordinates> coordinates)
            : this(null, coordinates)
        {
        }

        public GeoJsonMultiPolygon(GeoJsonObjectArgs<TCoordinates> args, GeoJsonMultiPolygonCoordinates<TCoordinates> coordinates)
            : base(args)
        {
            if (coordinates == null)
            {
                throw new ArgumentNullException("coordinates");
            }

            _coordinates = coordinates;
        }

        // public properties
        public GeoJsonMultiPolygonCoordinates<TCoordinates> Coordinates
        {
            get { return _coordinates; }
        }

        public override GeoJsonObjectType Type
        {
            get { return GeoJsonObjectType.MultiPolygon; }
        }
    }
}
