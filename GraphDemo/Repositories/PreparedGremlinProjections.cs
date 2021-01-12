namespace GraphDemo.Repositories
{
    public static class PreparedGremlinProjections
    {
        public const string StaffProjection =
            "project('id', 'label', 'partitionKey', 'name')" +
            ".by('id')" +
            ".by('label')" +
            ".by('partitionKey')" +
            ".by('name')";

        public const string AirportProjection =
            "project('id', 'label', 'partitionKey', 'code', 'city', 'runways', 'lat', 'lon')" +
            ".by('id')" +
            ".by('label')" +
            ".by('partitionKey')" +
            ".by('code')" +
            ".by('city')" +
            ".by('runways')" +
            ".by('lat')" +
            ".by('lon')";

        public static string OutboundRouteProjection =
            "as('startV')" +
            ".outE('route')" +
            ".as('edge')" +
            ".inV()" +
            ".as('endV')" +
            ".project('start', 'distance', 'destination')" +
            $".by(select('startV').{AirportProjection})" +
            ".by(select('edge').values('distance'))" +
            $".by(select('endV').{AirportProjection})";

        public static string InboundRouteProjection =
            "as('endV')" +
            ".inE('route')" +
            ".as('edge')" +
            ".outV()" +
            ".as('startV')" +
            ".project('start', 'distance', 'destination')" +
            $".by(select('startV').{AirportProjection})" +
            ".by(select('edge').values('distance'))" +
            $".by(select('endV').{AirportProjection})";
    }
}
