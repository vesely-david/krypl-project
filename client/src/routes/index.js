import CoreLayout from '../layouts/PageLayout/PageLayout'
import LoginRoute from './Login'
import Overview from './Overview'
import Real from './Real'
import Paper from './Paper'
import Backtest from './Backtest'
import Strategy from './Strategy'
import { getTokenIfValid } from '../helpers'

function requireAuth (nextState, replace, cb) {
  if (!getTokenIfValid()) {
    replace({
      pathname: '/login'
    })
  }
  cb()
}

function dontRequireAuth (nextState, replace, cb) {
  if (getTokenIfValid()) {
    replace({
      pathname: '/'
    })
  }
  cb()
}

export const createRoutes = (store) => ({
  path        : '/',
  component   : CoreLayout,
  childRoutes : [
    {
      onEnter: requireAuth,
      indexRoute  : Overview(store),
      childRoutes: [
        Real(store),
        Paper(store),
        Backtest(store),
        Strategy(store),
      ]
    },
    {
      onEnter: dontRequireAuth,
      childRoutes: [
        LoginRoute(store),
      ]
    }
  ]
})

export default createRoutes
