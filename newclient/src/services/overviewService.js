import axios from 'axios';


const getPaperOverview = () => get(`${document.masterApi}/strategies/papertesting/overview`, );
const getPaperStrategies = () => get(`${document.masterApi}/strategies/papertesting`, );
const registerPaperStrategy = (name, exchange, description, assets) => 
  post(`${document.masterApi}/strategies`, {
    name,
    exchange,
    description,
    tradingMode: 'PaperTesting',
    assets,
  })

async function get(url){
  const response = await axios.get(url, {
    headers: {
      'Accept': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    },
  })
  return response.data;
}

async function post(url, data){
  const response = await axios.post(url, JSON.stringify(data), {
    headers: {
      'Accept': 'application/json',
      'Content-type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    },
  })
  return response.data;
}

export const overviewService = {
  getPaperOverview,
  getPaperStrategies,
  registerPaperStrategy,
}