import React, { useState } from 'react'
import { Users, Globe, Settings, PlusCircle, Activity, Bell } from 'lucide-react'
import ArchitectList from './ArchitectList'
import ProjectPortal from './ProjectPortal'
import ArchitectSettings from './ArchitectSettings'
import RealTimeStream from './RealTimeStream'
import Notifications from './Notifications'
import AddArchitectPopup from './AddArchitectPopup'

const GameEditorInterface = () => {
  const [activePage, setActivePage] = useState('architects')
  const [showAddArchitect, setShowAddArchitect] = useState(false)

  const renderPage = () => {
    switch (activePage) {
      case 'architects':
        return <ArchitectList />
      case 'project':
        return <ProjectPortal />
      case 'settings':
        return <ArchitectSettings />
      case 'stream':
        return <RealTimeStream />
      case 'notifications':
        return <Notifications />
      default:
        return <ArchitectList />
    }
  }

  return (
    <div className="fixed inset-0 flex items-center justify-center bg-black bg-opacity-50">
      <div className="bg-black border border-white w-[700px] h-[400px] text-white text-sm overflow-hidden flex flex-col">
        <div className="flex justify-between items-center p-2 border-b border-white/20">
          <div className="flex space-x-2">
            <ToolbarButton icon={<Users size={16} />} onClick={() => setActivePage('architects')} active={activePage === 'architects'} label="Architects"/>
            <ToolbarButton icon={<Globe size={16} />} onClick={() => setActivePage('project')} active={activePage === 'project'} label="Project"/>
            <ToolbarButton icon={<Settings size={16} />} onClick={() => setActivePage('settings')} active={activePage === 'settings'} label="Settings"/>
          </div>
          <div className="flex space-x-2">
            <ToolbarButton icon={<PlusCircle size={16} />} label="Add Architect" onClick={() => setShowAddArchitect(true)} />
            <ToolbarButton icon={<Activity size={16} />} label="Stream" onClick={() => setActivePage('stream')} active={activePage === 'stream'} />
            <ToolbarButton icon={<Bell size={16} />} label="Notifications" onClick={() => setActivePage('notifications')} active={activePage === 'notifications'} />
          </div>
        </div>
        <div className="flex-grow overflow-auto p-2">
          {renderPage()}
        </div>
      </div>
      {showAddArchitect && <AddArchitectPopup onClose={() => setShowAddArchitect(false)} />}
    </div>
  )
}

const ToolbarButton = ({ icon, label, onClick, active = false }) => (
  <button
    className={`p-1 rounded flex items-center ${
      active ? 'bg-white text-black' : 'text-white hover:bg-white/10'
    }`}
    onClick={onClick}
  >
    {icon}
    {label && <span className="ml-1 text-xs">{label}</span>}
  </button>
)

export default GameEditorInterface