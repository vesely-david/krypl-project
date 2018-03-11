import { authHeader } from '../helpers'

export const balanceService = {
  mirrorRealAssets,
  getExchangesOverview
}

const addr = 'http://localhost:54849/api/client/'

async function mirrorRealAssets (exchange) {
  const requestOptions = {
    method: 'POST',
    headers: authHeader()
  }
  var response = await fetch(addr + 'mirrorRealAssets/' + exchange, requestOptions)

  if (response.ok) {
    return null
  }
  var error = await response.json()
  throw new Error(error)
}

async function getExchangesOverview () {
  const requestOptions = {
    method: 'GET',
    headers: authHeader()
  }
  var response = await fetch(addr + 'exchangesOverview', requestOptions)

  if (response.ok) {
    return response.json()
  }
  throw new Error(response.status)
}
