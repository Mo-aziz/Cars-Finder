import React, { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import { engineService } from '../services/engineService'
import { useAuth } from '../context/AuthContext'
import Loading from '../components/Loading'
import Alert from '../components/Alert'

const EnginesPage = () => {
  const [engines, setEngines] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [success, setSuccess] = useState('')
  const { isAdmin, isEmployee } = useAuth()

  useEffect(() => {
    fetchEngines()
  }, [])

  const fetchEngines = async () => {
    try {
      setLoading(true)
      const data = await engineService.getAll()
      setEngines(data)
    } catch (err) {
      setError(err.message || 'Failed to fetch engines')
    } finally {
      setLoading(false)
    }
  }

  const handleDelete = async (id) => {
    if (window.confirm('Are you sure you want to delete this engine?')) {
      try {
        await engineService.delete(id)
        setSuccess('Engine deleted successfully!')
        setEngines(engines.filter(engine => engine.id !== id))
        setTimeout(() => setSuccess(''), 3000)
      } catch (err) {
        setError(err.message || 'Failed to delete engine')
      }
    }
  }

  if (loading) return <Loading />

  return (
    <div className="container">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-4xl font-bold">Engines</h1>
        {(isAdmin || isEmployee) && (
          <Link to="/engines/new" className="btn-primary">
            Add Engine
          </Link>
        )}
      </div>

      <Alert message={error} type="error" onClose={() => setError('')} />
      <Alert message={success} type="success" onClose={() => setSuccess('')} />

      {engines.length === 0 ? (
        <div className="text-center py-12 animate-fade-in">
          <p className="text-gray-400 text-lg">No engines found.</p>
        </div>
      ) : (
        <div className="grid gap-4">
          {engines.map((engine, index) => (
            <div 
              key={engine.id} 
              className="card flex justify-between items-center"
              style={{ animationDelay: `${index * 50}ms` }}
            >
              <div>
                <h2 className="text-xl font-bold text-blue-400">{engine.type}</h2>
                <p className="text-gray-400 mt-1">{engine.horsePower} HP</p>
              </div>
              <div className="flex gap-2">
                <Link
                  to={`/engines/${engine.id}`}
                  className="btn-secondary text-sm"
                >
                  View
                </Link>
                {isAdmin && (
                  <>
                    <Link
                      to={`/engines/${engine.id}/edit`}
                      className="btn-secondary text-sm"
                    >
                      Edit
                    </Link>
                    <button
                      onClick={() => handleDelete(engine.id)}
                      className="btn-danger text-sm"
                    >
                      Delete
                    </button>
                  </>
                )}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  )
}

export default EnginesPage
