import React from 'react'

const ArchitectSettings = () => {
  return (
    <div className="space-y-2">
      <h2 className="text-lg font-bold">Architect Settings</h2>
      <div className="border border-white/20 p-2 rounded space-y-2">
        <label className="flex items-center justify-between text-xs">
          <span>Collaborative Mode</span>
          <input type="checkbox" className="form-checkbox h-3 w-3" />
        </label>
        <label className="flex items-center justify-between text-xs">
          <span>Auto-save (min)</span>
          <input type="number" className="form-input bg-transparent border border-white/20 w-12 h-6 text-xs" min="1" max="60" defaultValue="5" />
        </label>
        <label className="flex items-center justify-between text-xs">
          <span>Default LLM</span>
          <select className="form-select bg-transparent border border-white/20 h-6 text-xs">
            <option>GPT-4</option>
            <option>GPT-3.5</option>
            <option>Claude-2</option>
          </select>
        </label>
      </div>
    </div>
  )
}

export default ArchitectSettings
