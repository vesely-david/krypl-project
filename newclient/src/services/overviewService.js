import axios from 'axios';
import{
  PAPER_TESTING,
  REAL
} from '../common/tradingModes';


const getPaperOverview = () => get(`${document.masterApi}/strategies/papertesting/overview`);
const getRealOverview = () => get(`${document.masterApi}/strategies/real/overview`);


const getPaperStrategies = () => get(`${document.masterApi}/strategies/papertesting`);
const getRealStrategies = () => get(`${document.masterApi}/strategies/real`);


const registerPaperStrategy = (name, exchange, description, assets) => 
  post(`${document.masterApi}/strategies`, {
    name,
    exchange,
    description,
    tradingMode: PAPER_TESTING,
    assets,
  })
const registerRealStrategy = (name, exchange, description, assets) => 
  post(`${document.masterApi}/strategies`, {
    name,
    exchange,
    description,
    tradingMode: REAL,
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
  getRealOverview,
  getRealStrategies,
  registerRealStrategy,
}