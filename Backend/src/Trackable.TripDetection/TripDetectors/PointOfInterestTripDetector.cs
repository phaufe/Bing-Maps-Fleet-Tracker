﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackable.Common;
using Trackable.Configurations;
using Trackable.Repositories;
using Trackable.Models;
using Trackable.TripDetection.Components;
using Trackable.TripDetection.Helpers;
using Trackable.TripDetection.Modules;

namespace Trackable.TripDetection
{
    internal class PointOfInterestTripDetector : ITripDetector
    {
        private readonly IConfigurationRepository configurationRepository;
        private readonly ILocationRepository locationRepository;
        private readonly ITripRepository tripRepository;
        private readonly ITrackingPointRepository trackingPointRepository;
        private readonly string bingMapsKey;

        public TripDetectorType TripDetectorType
        {
            get
            {
                return TripDetectorType.SimplePointOfInterst;
            }
        }

        internal PointOfInterestTripDetector(
            IConfigurationRepository configurationRepository,
            ILocationRepository locationRepository,
            ITripRepository tripRepository,
            ITrackingPointRepository trackingPointRepository,
            string bingMapsKey)
        {
            this.configurationRepository = configurationRepository;
            this.locationRepository = locationRepository;
            this.tripRepository = tripRepository;
            this.trackingPointRepository = trackingPointRepository;
            this.bingMapsKey = bingMapsKey;
        }


        public IEnumerable<IModuleLoader> GetModuleLoaders()
        {
            var manager = new ModuleConfigurationManager(
               configurationRepository,
               ConfigurationExtensions.GetAssemblyNamespace(),
               nameof(PointOfInterestTripDetector));

            return new List<IModuleLoader>
            {
                manager.LoadModuleAsync<PointLoaderModule>(this.trackingPointRepository),
                manager.LoadModuleAsync<NoiseRemovalModule>(),
                manager.LoadModuleAsync<ClusteredStopDetectorModule>(),
                manager.LoadModuleAsync<EdgeStopDetectorModule>(),
                manager.LoadModuleAsync<BlackoutStopDetectorModule>(),
                manager.LoadModuleAsync<StopToLocationMapperModule>(this.locationRepository),
                manager.LoadModuleAsync<LegCandidateExtractorModule>(),
                manager.LoadModuleAsync<PoiTripExtractorModule>(this.locationRepository),
                manager.LoadModuleAsync<PersistenceModule>(this.trackingPointRepository, this.tripRepository, this.locationRepository, this.bingMapsKey)
            };
        }
    }
}
