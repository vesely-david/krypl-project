import axios from 'axios';

async function getStrategyTrades(id){
  const response = await axios.get(
    `${document.masterApi}/strategy/${id}/trades`, {
    headers: {
      'Accept': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    },
  })
  return response.data;
}

async function getStrategyHistory(id){
  const response = await axios.get(
    `${document.masterApi}/strategy/${id}/history`, {
    headers: {
      'Accept': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    },
  })
  return response.data;
}

async function getStrategy(id, tradingmode){
  const response = await axios.get(
    `${document.masterApi}/strategies/${tradingmode}/${id}`, {
    headers: {
      'Accept': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    },
  })
  return response.data;
}

async function stopStrategy(strategyId){
  const response = await axios.post(
    `${document.masterApi}/strategy/${strategyId}/stop`, 
    JSON.stringify({}), {
    headers: {
      'Accept': 'application/json',
      'Content-type': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    },
  })
  return response.data;
}

export const strategyService = {
  getStrategy,
  getStrategyTrades,
  getStrategyHistory,
  stopStrategy,
}