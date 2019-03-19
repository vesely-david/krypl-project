import {
  GET_STRATEGY_OVERVIEW,
  GET_STRATEGY_TRADES,
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
}

const initialState = {
  overviews: {},
  trades: {},
  strategyOverviewFetching: false,
  strategyTradesFetching: false,
}

export default function userReducer (state = initialState, action) {
  const handler = ACTION_HANDLERS[action.type]
  return handler ? handler(state, action) : state
}
