import {
  GET_BACK_OVERVIEW,
  GET_BACK_STRATEGIES,
  GET_BACK_STRATEGY,
} from '../actions/types';


const ACTION_HANDLERS = {
  [`${GET_BACK_STRATEGIES}_PENDING`] : (state, action) => {
    return {
      ...state, 
      strategiesFetching: true
    }
  },
  [`${GET_BACK_STRATEGIES}_FULFILLED`] : (state, action) => {
    return {
      ...state,
      strategiesFetching: false,
      strategies: action.payload.strategies,
      pagination: action.payload.pagination,
    }
  },
  [`${GET_BACK_STRATEGIES}_REJECTED`] : (state, action) => {
    return {
      ...state, 
      strategiesFetching: false
    }
  },
  //====================================================     
  [`${GET_BACK_STRATEGY}_PENDING`] : (state, action) => {
    return {
      ...state, 
      strategiesFetching: true
    }
  },
  [`${GET_BACK_STRATEGY}_FULFILLED`] : (state, action) => {
    const original = state.strategies.find(o => o.id === action.payload.id);
    return {
      ...state,
      strategiesFetching: false,
      strategies: original ? 
      state.strategies.map(o => o.id === action.payload.id ? action.payload : o) : [action.payload],
      pagination: original ? state.pagination : {page: 0, perPage: 30, total: 0},
    }
  },
  [`${GET_BACK_STRATEGY}_REJECTED`] : (state, action) => {
    return {
      ...state, 
      strategiesFetching: false
    }
  },
  //====================================================       
  [`${GET_BACK_OVERVIEW}_PENDING`] : (state, action) => {
    return {...state, overviewFetching: true}
  },
  [`${GET_BACK_OVERVIEW}_FULFILLED`] : (state, action) => {
    return {
      ...state,
      overview: action.payload,
      overviewFetching: false
    }
  },
  [`${GET_BACK_OVERVIEW}_REJECTED`] : (state, action) => ({
    ...state, 
    overviewFetching: false
  }),
}


const initialState = {
strategies: [],
pagination: {page: 0, perPage: 30, total: 0},
overview: {},
registrationPending: false,
strategiesFetching: false,
overviewFetching: false,
}

export default function backReducer (state = initialState, action) {
  const handler = ACTION_HANDLERS[action.type]
  return handler ? handler(state, action) : state
}
