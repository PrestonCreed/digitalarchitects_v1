
import React, { useState } from 'react'
import ArchitectDetails from './ArchitectDetails'

const ArchitectList = () => {
  const [selectedArchitect, setSelectedArchitect] = useState(null)

  const architects = [
    { id: 1, name: 'Architect 1', status: 'Active', role: 'Environment Designer', llm: 'GPT-4' },
    { id: 2, name: 'Architect 2', status: 'Idle', role: 'Character Creator', llm: 'Claude-2' },
    { id: 3, name: 'Architect 3', status: 'Active', role: 'Quest Designer', llm: 'GPT-3.5' },
  ]

  return (
    <div className="space-y-2">
      {selectedArchitect ? (
        <ArchitectDetails architect={selectedArchitect} onClose={() => setSelectedArchitect(null)} />
      ) : (
        architects.map((architect) => (
          <div
            key={architect.id}
            className="border border-white/20 p-2 rounded cursor-pointer hover:bg-white/5"
            onClick={() => setSelectedArchitect(architect)}
          >
            <div className="flex justify-between items-center">
              <span className="font-bold">{architect.name}</span>
              <span className={`text-xs ${architect.status === 'Active' ? 'text-green-400' : 'text-yellow-400'}`}>
                {architect.status}
              </span>
            </div>
            <div className="text-xs text-white/70">{architect.role} | {architect.llm}</div>
          </div>
        ))
      )}
    </div>
  )
}

export default ArchitectList