import {
  GET_STRATEGY_OVERVIEW,
  GET_STRATEGY_TRADES,
  GET_STRATEGY_VALUE_HISTORY,
} from '../actions/types';


const ACTION_HANDLERS = {   
  [`${GET_STRATEGY_OVERVIEW}_PENDING`] : (state, action) => {
    return {
      ...state, 
      strategyOverviewFetching: true
    }
  },
  [`${GET_STRATEGY_OVERVIEW}_FULFILLED`] : (state, action) => {
    return {
      ...state,
      strategyOverviewFetching: false,
      overviews: {
        ...state.overviews,
        [action.payload.id] : action.payload,
      }
    }
  },
  [`${GET_STRATEGY_OVERVIEW}_REJECTED`] : (state, action) => {
    return {
      ...state, 
      strategyOverviewFetching: false
    }
  },
  //====================================================   
  [`${GET_STRATEGY_TRADES}_PENDING`] : (state, action) => {
    return {
      ...state, 
      strategyTradesFetching: true
    }
  },
  [`${GET_STRATEGY_TRADES}_FULFILLED`] : (state, action) => {
    return {
      ...state,
      strategyTradesFetching: false,
      trades: {
        ...state.trades,
        [action.payload.id] : action.payload.trades,
      }
    }
  },
  [`${GET_STRATEGY_TRADES}_REJECTED`] : (state, action) => {
    return {
      ...state, 
      strategyTradesFetching: false
    }
  },
  //====================================================   
  [`${GET_STRATEGY_VALUE_HISTORY}_PENDING`] : (state, action) => {
    return {
      ...state, 
      historyFetching: true
    }
  },
  [`${GET_STRATEGY_VALUE_HISTORY}_FULFILLED`] : (state, action) => {
    return {
      ...state,
      historyFetching: false,
      histories: {
        ...state.histories,
        [action.payload.id] : action.payload.history,
      }
    }
  },
  [`${GET_STRATEGY_VALUE_HISTORY}_REJECTED`] : (state, action) => {
    return {
      ...state, 
      historyFetching: false
    }
  },  
}

const initialState = {
  overviews: {},
  trades: {},
  histories: {},
  strategyOverviewFetching: false,
  strategyTradesFetching: false,
  historyFetching: false,
}

export default function userReducer (state = initialState, action) {
  const handler = ACTION_HANDLERS[action.type]
  return handler ? handler(state, action) : state
}
