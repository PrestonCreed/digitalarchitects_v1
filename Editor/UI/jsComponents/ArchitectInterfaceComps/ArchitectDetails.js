import React from 'react'
import { X, PenTool } from 'lucide-react'

const ArchitectDetails = ({ architect, onClose }) => {
  return (
    <div className="space-y-4">
      <div className="flex justify-between items-center">
        <h2 className="text-lg font-bold">{architect.name}</h2>
        <button onClick={onClose} className="text-white/50 hover:text-white">
          <X size={16} />
        </button>
      </div>
      
      <div className="space-y-2">
        <DetailSection title="Main Goal">
          Design and maintain the game's environmental elements to create an immersive and dynamic world.
        </DetailSection>

        <DetailSection title="LLM Usage">
          <div className="flex justify-between items-center">
            <span>Model: {architect.llm}</span>
            <span>Tokens used: 1,234,567</span>
          </div>
        </DetailSection>

        <DetailSection title="Settings">
          <div className="space-y-1">
            <Setting name="Creativity" value={75} />
            <Setting name="Consistency" value={90} />
            <Setting name="Response Length" value={60} />
          </div>
        </DetailSection>

        <DetailSection title="Usage Monitoring">
          <UsageChart />
        </DetailSection>

        <DetailSection title="Status">
          <span className={`text-sm ${architect.status === 'Active' ? 'text-green-400' : 'text-yellow-400'}`}>
            {architect.status}
          </span>
          <p className="text-xs text-white/70 mt-1">Last active: 5 minutes ago</p>
        </DetailSection>

        <DetailSection title="Tool Repository">
          <div className="grid grid-cols-2 gap-2">
            <Tool name="Terrain Generator" />
            <Tool name="NPC Creator" />
            <Tool name="Quest Builder" />
            <Tool name="Dialogue System" />
          </div>
        </DetailSection>
      </div>
    </div>
  )
}

const DetailSection = ({ title, children }) => (
  <div className="border border-white/20 p-2 rounded">
    <h3 className="text-sm font-bold mb-1">{title}</h3>
    <div className="text-xs">{children}</div>
  </div>
)

const Setting = ({ name, value }) => (
  <div className="flex justify-between items-center">
    <span>{name}</span>
    <div className="w-24 h-2 bg-white/20 rounded-full overflow-hidden">
      <div className="h-full bg-white" style={{ width: `${value}%` }}></div>
    </div>
  </div>
)

const UsageChart = () => (
  <svg className="w-full h-16" viewBox="0 0 100 20">
    <polyline
      fill="none"
      stroke="white"
      strokeWidth="0.5"
      points="0,10 10,8 20,12 30,6 40,14 50,4 60,16 70,2 80,18 90,8 100,10"
    />
  </svg>
)

const Tool = ({ name }) => (
  <div className="flex items-center space-x-1 text-xs">
    <PenTool size={12} />
    <span>{name}</span>
  </div>
)

export default ArchitectDetails
