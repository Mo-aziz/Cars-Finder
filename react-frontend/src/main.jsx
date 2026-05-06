import React from 'react'
import ReactDOM from 'react-dom/client'
import App from './App'
import './index.css'

const root = ReactDOM.createRoot(document.getElementById('root'))

// Only use StrictMode in development for extra checks, but it causes double-renders
// For better performance testing, we can disable it
root.render(
  <App />
)
