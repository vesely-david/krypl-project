import { authHeader, authHeaderJsonType } from '../helpers'

export const strategyService = {
  getRealStrategies,
  getRealOverview,
  getPaperStrategies,
  getBacktestStrategies,
  getStrategy,
  stopStrategy,
  registerReal,
  forgetAllRealNews,
  forgetRealNews,
  forgetAllPaperNews,
  forgetPaperNews,
  forgetAllBacktestNews,
  forgetBacktestNews,
  getMainOverview,
}

const addr = 'http://localhost:54849/api/client/'

async function getRealStrategies () {
  const requestOptions = {
    method: 'GET',
    headers: authHeader()
  }
  var response = await fetch(addr + 'realStrategies', requestOptions)

  if (response.ok) {
    return response.json()
  }
  throw new Error(response.status)
}

async function getRealOverview () {
  const requestOptions = {
    method: 'GET',
    headers: authHeader()
  }
  var response = await fetch(addr + 'realOverview', requestOptions)

  if (response.ok) {
    return response.json()
  }
  throw new Error(response.status)
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

async function registerReal (name, exchange, description, assets) {
  const requestOptions = {
    method: 'POST',
    headers: authHeaderJsonType(),
    body: JSON.stringify({ name, exchangeId: exchange, description, assets })
  }

  const response = await fetch(`${addr}register`, requestOptions)
  if (response.ok) {
    const parsed = await response.json()
    return parsed
  }
  throw new Error(response.status)
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

function forgetAllRealNews () {
}

function forgetRealNews () {
}

function forgetAllPaperNews () {
}

function forgetPaperNews () {
}

function forgetAllBacktestNews () {
}

function forgetBacktestNews () {
}

async function getMainOverview () {
  const requestOptions = {
    method: 'GET',
    headers: authHeader()
  }
  var response = await fetch(addr + 'mainOverview', requestOptions)

  if (response.ok) {
    return response.json()
  }
  throw new Error(response.status)
}
