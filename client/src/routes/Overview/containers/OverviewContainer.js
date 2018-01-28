import React from 'react'
import { connect } from 'react-redux'

const OverviewContainer = () => {
  return (
    <h1>Overview</h1>
  )
}

const mapDispatchToProps = {
}

const mapStateToProps = (state) => ({
})

export default connect(mapStateToProps, mapDispatchToProps)(OverviewContainer)
