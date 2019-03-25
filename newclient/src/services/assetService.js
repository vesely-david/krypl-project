import axios from 'axios'

const updatePaperAssets = (assets) => updateAssets(`${document.masterApi}/assets/paper`, assets)

async function getAssets(){
  const response = await axios.get(
    `${document.masterApi}/assets`,
    {
      headers: {
        'Accept': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      },
    }
  )
  return response.data;
}

async function updateAssets(url, assets){
  const response = await axios.post(
    url,
    JSON.stringify(assets),
    {
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      },
    }
  )
  return response.data;
}

async function updateRealAssets(exchangeId){
  const response = await axios.post(
    `${document.masterApi}/assets/real/${exchangeId}`,
    null, {
      headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      },
    }
  )
  return response.data;
}

export const assetService = {
  getAssets,
  updatePaperAssets,
  updateRealAssets,
}