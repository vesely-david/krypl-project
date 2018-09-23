
export const marketDataService = {
  fetchMarketData,
}

const addr = 'https://marketdata.kryplproject.cz/'

async function fetchMarketData () {
  const requestOptions = {
    method: 'GET',
  }
  var response = await fetch(addr + 'exchanges', requestOptions)

  if (response.ok) {
    return response.json()
  }
  throw new Error(response.status)
}
