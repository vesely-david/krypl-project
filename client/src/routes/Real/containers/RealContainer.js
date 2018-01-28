import React from 'react'
import { connect } from 'react-redux'

const RealContainer = () => {
  return (
    <h1>Real</h1>
  )
}

const mapDispatchToProps = {
}

const mapStateToProps = (state) => ({
})

export default connect(mapStateToProps, mapDispatchToProps)(RealContainer)
