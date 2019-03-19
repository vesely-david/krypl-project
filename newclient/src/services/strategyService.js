import axios from 'axios';

async function getStrategyOverview(id){
  const response = await axios.get(
    `${document.masterApi}/strategy/${id}/overview`, {
    headers: {
      'Accept': 'application/json',
      'Authorization': `Bearer ${localStorage.getItem('token')}`
    },
  })
  return response.data;
}

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


export const strategyService = {
  getStrategyOverview,
  getStrategyTrades,
  getStrategyHistory,
}