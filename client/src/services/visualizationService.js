import { authHeader } from '../helpers'

export const visualizationService = {
  fetchStrategyOverview,
  fetchStrategyTrades
}

const addr = 'https://api.kryplproject.cz/client/'
//const addr = 'http://localhost:54849/client/'

async function fetchStrategyOverview (id) {
  const requestOptions = {
    method: 'GET',
    headers: authHeader()
  }
  var response = await fetch(addr + 'strategyOverview/' + id, requestOptions)

  if (response.ok) {
    return response.json()
  }
  throw new Error(response.status)
}

async function fetchStrategyTrades (id) {
  const requestOptions = {
    method: 'GET',
    headers: authHeader()
  }
  var response = await fetch(addr + 'strategyTrades/' + id, requestOptions)

  if (response.ok) {
    return response.json()
  }
  throw new Error(response.status)
}
