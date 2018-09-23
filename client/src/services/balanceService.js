import { authHeader, authHeaderJsonType } from '../helpers'

export const balanceService = {
  mirrorRealAssets,
  mirrorPaperAssets
}

const addr = 'https://api.kryplproject.cz/client/'
//const addr = 'http://localhost:54849/client/'

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

async function mirrorPaperAssets (exchange, assets) {
  const requestOptions = {
    method: 'POST',
    headers: authHeaderJsonType(),
    body: JSON.stringify(assets)

  }
  var response = await fetch(addr + 'mirrorPaperAssets/' + exchange, requestOptions)
  if (response.ok) {
    return null
  }
  var error = await response.json()
  throw new Error(error)
}
