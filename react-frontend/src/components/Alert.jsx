import React from 'react'

const Alert = ({ message, type = 'info', onClose }) => {
  if (!message) return null

  const bgColor = {
    success: 'bg-green-900 text-green-100 border-green-700',
    error: 'bg-red-900 text-red-100 border-red-700',
    warning: 'bg-yellow-900 text-yellow-100 border-yellow-700',
    info: 'bg-blue-900 text-blue-100 border-blue-700'
  }[type] || bgColor.info

  return (
    <div className={`border rounded p-4 mb-4 ${bgColor}`}>
      <div className="flex justify-between items-center">
        <p>{message}</p>
        {onClose && (
          <button
            onClick={onClose}
            className="text-xl font-bold cursor-pointer"
          >
            ×
          </button>
        )}
      </div>
    </div>
  )
}

export default Alert
