import HomePage from 'components/home-page'
import DetailPage from 'components/detail'
import AboutUs from 'components/about-us'
import ListPage from 'components/list-page'
import JoinUs from 'components/join-us'

export const routes = [
  { name: 'home', path: '/', component: HomePage, display: 'Home', icon: 'home' },
  { name: 'detail', path: '/detail/:id', component: DetailPage, display: 'Detail' },
  { name: 'about-us', path: '/about-us', component: AboutUs, display: 'AboutUs', icon: 'about-us' },
  { name: 'list', path: '/list', component: ListPage, display: 'List', icon: 'list' },
  { name: 'join-us', path: '/join-us', component: JoinUs, display: 'JoinUs', icon: 'join-us' }
]
