import React, { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import { carProfileService } from '../services/carProfileService'
import { useAuth } from '../context/AuthContext'
import Loading from '../components/Loading'
import Alert from '../components/Alert'

const CarProfilesPage = () => {
  const [profiles, setProfiles] = useState([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState('')
  const [success, setSuccess] = useState('')
  const { isAdmin, isEmployee } = useAuth()
  const defaultCarIcon = '/default-car.svg'

  useEffect(() => {
    fetchProfiles()
  }, [])

  const fetchProfiles = async () => {
    try {
      setLoading(true)
      const data = await carProfileService.getAll()
      setProfiles(data)
    } catch (err) {
      setError(err.message || 'Failed to fetch car profiles')
    } finally {
      setLoading(false)
    }
  }

  const handleDelete = async (id) => {
    if (window.confirm('Are you sure you want to delete this car profile?')) {
      try {
        await carProfileService.delete(id)
        setSuccess('Car profile deleted successfully!')
        setProfiles(profiles.filter(p => p.id !== id))
        setTimeout(() => setSuccess(''), 3000)
      } catch (err) {
        setError(err.message || 'Failed to delete car profile')
      }
    }
  }

  if (loading) return <Loading />

  return (
    <div className="container">
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-4xl font-bold">Car Profiles</h1>
        {(isAdmin || isEmployee) && (
          <Link to="/car-profiles/new" className="btn-primary">
            Add Car Profile
          </Link>
        )}
      </div>

      <Alert message={error} type="error" onClose={() => setError('')} />
      <Alert message={success} type="success" onClose={() => setSuccess('')} />

      {profiles.length === 0 ? (
        <div className="text-center py-12 animate-fade-in">
          <p className="text-gray-400 text-lg">No car profiles found.</p>
        </div>
      ) : (
        <div className="grid gap-4">
          {profiles.map((profile, index) => (
            <div 
              key={profile.id} 
              className="card flex justify-between items-center"
              style={{ animationDelay: `${index * 50}ms` }}
            >
              <div className="flex items-center gap-4">
                <img
                  src={profile.photoUrl || defaultCarIcon}
                  alt={profile.carName || 'Car profile image'}
                  className="w-16 h-16 rounded object-cover border border-gray-700"
                  onError={(e) => {
                    e.currentTarget.onerror = null
                    e.currentTarget.src = defaultCarIcon
                  }}
                />
                <div>
                  <h2 className="text-xl font-bold text-blue-400">
                    {profile.carName || `Car ID: ${profile.carId}`}
                  </h2>
                  <p className="text-gray-400 mt-1">
                    {profile.color} | ${profile.price?.toLocaleString() || 'N/A'}
                  </p>
                </div>
              </div>
              <div className="flex gap-2">
                <Link
                  to={`/car-profiles/${profile.id}`}
                  className="btn-secondary text-sm"
                >
                  View
                </Link>
                {(isAdmin || isEmployee) && (
                  <>
                    <Link
                      to={`/car-profiles/${profile.id}/edit`}
                      className="btn-secondary text-sm"
                    >
                      Edit
                    </Link>
                    <button
                      onClick={() => handleDelete(profile.id)}
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

export default CarProfilesPage
