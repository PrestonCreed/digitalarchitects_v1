import React from 'react'

const Notifications = () => {
  const notifications = [
    { type: 'warning', message: 'High resource usage detected in sector B7' },
    { type: 'info', message: 'Architect 2 completed character design for "Village Elder"' },
    { type: 'error', message: 'Failed to save changes in Project Portal' },
  ]

  return (
    <div className="space-y-2">
      <h2 className="text-lg font-bold">Notifications</h2>
      <div className="space-y-1">
        {notifications.map((notification, index) => (
          <div key={index} className={`text-xs p-2 rounded ${getNotificationColor(notification.type)}`}>
            {notification.message}
          </div>
        ))}
      </div>
    </div>
  )
}

const getNotificationColor = (type) => {
  switch (type) {
    case 'warning':
      return 'bg-yellow-500/20 border border-yellow-500/40'
    case 'error':
      return 'bg-red-500/20 border border-red-500/40'
    default:
      return 'bg-blue-500/20 border border-blue-500/40'
  }
}

export default Notificationsimport React from 'react'

const Notifications = () => {
  const notifications = [
    { type: 'warning', message: 'High resource usage detected in sector B7' },
    { type: 'info', message: 'Architect 2 completed character design for "Village Elder"' },
    { type: 'error', message: 'Failed to save changes in Project Portal' },
  ]

  return (
    <div className="space-y-2">
      <h2 className="text-lg font-bold">Notifications</h2>
      <div className="space-y-1">
        {notifications.map((notification, index) => (
          <div key={index} className={`text-xs p-2 rounded ${getNotificationColor(notification.type)}`}>
            {notification.message}
          </div>
        ))}
      </div>
    </div>
  )
}

const getNotificationColor = (type) => {
  switch (type) {
    case 'warning':
      return 'bg-yellow-500/20 border border-yellow-500/40'
    case 'error':
      return 'bg-red-500/20 border border-red-500/40'
    default:
      return 'bg-blue-500/20 border border-blue-500/40'
  }
}

export default Notifications