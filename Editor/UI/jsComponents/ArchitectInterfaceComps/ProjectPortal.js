import React from 'react'
import { BarChart, Map, Users, Database, Globe, Cpu } from 'lucide-react'

const ProjectPortal = () => {
  return (
    <div className="space-y-6">
      <h2 className="text-2xl font-bold">Project Portal</h2>
      
      <div className="grid grid-cols-3 gap-6">
        <DataStreams />
        <SpatialDynamics />
        <PopulationStats />
      </div>
      
      <div className="grid grid-cols-2 gap-6">
        <EnvironmentMapping />
        <TechnicalOverview />
      </div>
    </div>
  )
}

const DataStreams = () => (
  <div>
    <h3 className="text-lg font-semibold mb-2 flex items-center">
      <Database className="mr-2" size={20} />
      Data Streams
    </h3>
    <div className="space-y-2">
      <DataItem label="Player Behavior" value="1.2M samples/day" />
      <DataItem label="NPC Interactions" value="500K logs/day" />
      <DataItem label="Quest Completions" value="50K/day" />
      <DataItem label="Combat Encounters" value="200K/day" />
    </div>
  </div>
)

const SpatialDynamics = () => (
  <div>
    <h3 className="text-lg font-semibold mb-2 flex items-center">
      <Map className="mr-2" size={20} />
      Spatial Dynamics
    </h3>
    <div className="space-y-2">
      <DataItem label="World Size" value="10,000 km²" />
      <DataItem label="Biomes" value="5 types" />
      <DataItem label="POIs" value="250 locations" />
      <DataItem label="Dynamic Weather" value="4 systems" />
    </div>
  </div>
)

const PopulationStats = () => (
  <div>
    <h3 className="text-lg font-semibold mb-2 flex items-center">
      <Users className="mr-2" size={20} />
      Population Stats
    </h3>
    <div className="space-y-2">
      <DataItem label="Total Architects" value="25" />
      <DataItem label="Active Now" value="18" />
      <DataItem label="Avg. Daily Active" value="22" />
      <DataItem label="New This Week" value="3" />
    </div>
  </div>
)

const EnvironmentMapping = () => (
  <div>
    <h3 className="text-lg font-semibold mb-2 flex items-center">
      <Globe className="mr-2" size={20} />
      Environment Mapping
    </h3>
    <div className="grid grid-cols-2 gap-4">
      <div className="space-y-2">
        <DataItem label="Terrain Types" value="8" />
        <DataItem label="Flora Species" value="120" />
        <DataItem label="Fauna Species" value="75" />
      </div>
      <div className="space-y-2">
        <DataItem label="Climate Zones" value="4" />
        <DataItem label="Day/Night Cycle" value="4 hours" />
        <DataItem label="Seasons" value="Dynamic" />
      </div>
    </div>
  </div>
)

const TechnicalOverview = () => (
  <div>
    <h3 className="text-lg font-semibold mb-2 flex items-center">
      <Cpu className="mr-2" size={20} />
      Technical Overview
    </h3>
    <div className="grid grid-cols-2 gap-4">
      <div className="space-y-2">
        <DataItem label="Engine" value="Unreal Engine 5" />
        <DataItem label="AI System" value="Custom Neural Net" />
      </div>
      <div className="space-y-2">
        <DataItem label="Rendering" value="Ray Tracing" />
        <DataItem label="Networking" value="Dedicated Servers" />
      </div>
    </div>
  </div>
)

const DataItem = ({ label, value }) => (
  <div className="flex justify-between items-center">
    <span className="text-sm text-gray-300">{label}</span>
    <span className="text-sm font-medium">{value}</span>
  </div>
)

export default ProjectPortal