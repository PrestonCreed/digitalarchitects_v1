import React, { useState } from 'react'
import { X } from 'lucide-react'

const AddArchitectPopup = ({ onClose }) => {
  const [name, setName] = useState('')
  const [goal, setGoal] = useState('')
  const [dataTypes, setDataTypes] = useState([])
  const [creativity, setCreativity] = useState(50)
  const [reviewRequired, setReviewRequired] = useState(false)

  const handleSubmit = (e) => {
    e.preventDefault()
    console.log({ name, goal, dataTypes, creativity, reviewRequired })
    onClose()
  }

  const handleDataTypeChange = (e) => {
    const { value, checked } = e.target
    if (checked) {
      setDataTypes([...dataTypes, value])
    } else {
      setDataTypes(dataTypes.filter(type => type !== value))
    }
  }

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center">
      <div className="bg-black border border-white w-[400px] text-white text-sm overflow-hidden">
        <div className="flex justify-between items-center p-2 border-b border-white/20">
          <h2 className="text-lg font-bold">Add New Architect</h2>
          <button onClick={onClose} className="text-white/50 hover:text-white">
            <X size={16} />
          </button>
        </div>
        <form onSubmit={handleSubmit} className="p-4 space-y-4">
          <div>
            <label htmlFor="name" className="block text-xs mb-1">Name (Optional)</label>
            <input
              type="text"
              id="name"
              value={name}
              onChange={(e) => setName(e.target.value)}
              className="w-full bg-transparent border border-white/20 rounded px-2 py-1 text-sm"
              placeholder="Architect name"
            />
          </div>
          <div>
            <label htmlFor="goal" className="block text-xs mb-1">Goal</label>
            <textarea
              id="goal"
              value={goal}
              onChange={(e) => setGoal(e.target.value)}
              className="w-full bg-transparent border border-white/20 rounded px-2 py-1 text-sm"
              rows={3}
              placeholder="Architect's main goal"
              required
            />
          </div>
          <div>
            <label className="block text-xs mb-1">Multimodal Data Processing</label>
            <div className="space-y-1">
              {['Text', 'Image', 'Audio', 'Video', 'Structured Data'].map((type) => (
                <label key={type} className="flex items-center">
                  <input
                    type="checkbox"
                    value={type}
                    checked={dataTypes.includes(type)}
                    onChange={handleDataTypeChange}
                    className="mr-2"
                  />
                  <span className="text-sm">{type}</span>
                </label>
              ))}
            </div>
          </div>
          <div>
            <label htmlFor="creativity" className="block text-xs mb-1">
              Creativity Scale: {creativity}
            </label>
            <input
              type="range"
              id="creativity"
              min="0"
              max="100"
              value={creativity}
              onChange={(e) => setCreativity(Number(e.target.value))}
              className="w-full"
            />
          </div>
          <div>
            <label className="flex items-center">
              <input
                type="checkbox"
                checked={reviewRequired}
                onChange={(e) => setReviewRequired(e.target.checked)}
                className="mr-2"
              />
              <span className="text-sm">Output Review Required</span>
            </label>
          </div>
          <button
            type="submit"
            className="w-full bg-white text-black font-bold py-2 px-4 rounded hover:bg-white/90"
          >
            Add Architect
          </button>
        </form>
      </div>
    </div>
  )
}

export default AddArchitectPopup

