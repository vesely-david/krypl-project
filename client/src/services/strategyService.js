import { authHeader } from '../helpers'

export const userService = {
  getRealStrategies,
  getPaperStrategies,
  getBacktestStrategies,
  getStrategy,
  stopStrategy,
  registerStrategy,
}

const addr = 'http://localhost:54849/api/client/'

function getRealStrategies () {
  const requestOptions = {
    method: 'GET',
    headers: authHeader()
  }
  return fetch(addr + 'realStrategies', requestOptions)
    .then(response => {
      if (!response.ok) {
        return Promise.reject(response.statusText)
      }
      return response.json()
    })
}

function getPaperStrategies () {
  const requestOptions = {
    method: 'GET',
    headers: authHeader()
  }
  return fetch(addr + 'paperStrategies', requestOptions)
    .then(response => {
      if (!response.ok) {
        return Promise.reject(response.statusText)
      }
      return response.json()
    })
}

function getBacktestStrategies () {
  const requestOptions = {
    method: 'GET',
    headers: authHeader()
  }
  return fetch(addr + 'backtestStrategies', requestOptions)
    .then(response => {
      if (!response.ok) {
        return Promise.reject(response.statusText)
      }
      return response.json()
    })
}

function getStrategy (strategyId) {
  const requestOptions = {
    method: 'GET',
    headers: authHeader()
  }
  return fetch(`${addr}strategies/${strategyId}`, requestOptions)
    .then(response => {
      if (!response.ok) {
        return Promise.reject(response.statusText)
      }
      return response.json()
    })
}

function registerStrategy (type, title, describtion, assetArray) {
  const requestOptions = {
    method: 'POST',
    headers: authHeader(),
    body: JSON.stringify({ type, title, describtion, assetArray })
  }
  return fetch(`${addr}register`, requestOptions)
    .then(response => {
      if (!response.ok) {
        return Promise.reject(response.statusText)
      }
      return response.json()
    })
}

function stopStrategy (strategyId) {
  const requestOptions = {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ strategyId })
  }
  return fetch(addr + 'stopStrategy', requestOptions)
    .then(response => {
      if (!response.ok) {
        return Promise.reject(response.statusText)
      }
      return response.json()
    })
}
