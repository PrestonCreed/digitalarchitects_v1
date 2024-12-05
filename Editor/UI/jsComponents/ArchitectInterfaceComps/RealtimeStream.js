import React from 'react'

const RealTimeStream = () => {
  const streamData = [
    { time: '10:15:30', event: 'Architect 1 modified terrain in sector A5' },
    { time: '10:15:45', event: 'Architect 2 created a new NPC: "Village Elder"' },
    { time: '10:16:00', event: 'Architect 3 updated quest: "The Lost Artifact"' },
  ]

  return (
    <div className="space-y-2">
      <h2 className="text-lg font-bold">Real-time Stream</h2>
      <div className="border border-white/20 p-2 rounded h-[300px] overflow-auto text-xs space-y-1">
        {streamData.map((item, index) => (
          <div key={index}>
            <span className="text-white/50">[{item.time}]</span> {item.event}
          </div>
        ))}
      </div>
    </div>
  )
}

export default RealTimeStream